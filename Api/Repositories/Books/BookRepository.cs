using Api.Data;
using Api.Data.Books;
using Api.Repositories.Interfaces.Books;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Books;

public class BookRepository(BookDbContext context, ILogger<BookRepository> logger) : IBookRepository
{
    public IQueryable<Book> GetAll()
    {
        var books = context.Books
            .Include(x => x.Authors).ThenInclude(a => a.Author)
            .Include(x => x.Genres).ThenInclude(g => g.Genre)
            .Include(x => x.Series)
            .Include(x => x.Keywords).ThenInclude(k => k.Keyword)
            .Include(x => x.Language);

        return books;
    }

    public int GetCount()
    {
        var count = context.Books.Count();
        return count;
    }

    public Task<Book?> GetByIdAsync(int id)
    {
        return context.Books.Include(b => b.Authors).ThenInclude(a => a.Author).FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<Book?> GetByTitleAsync(string title)
    {
        var booksQuery =
            context.Books.Where(x => x.Title.Equals(title, StringComparison.InvariantCultureIgnoreCase));

        return booksQuery.FirstOrDefaultAsync();
    }

    public async Task<Book> CreateAsync(Book book)
    {
        // var existedAuthor = await GetByTitleAsync(author.LastName, author.FirstName, author.MiddleName);
        // if (existedAuthor is not null)
        //     return existedAuthor;
        //
        // var newAuthor = await context.Authors.AddAsync(author);
        // // await context.SaveChangesAsync();
        // return newAuthor.Entity;
        return new Book();
    }

    public async Task DeleteAsync(int id)
    {
        var book = await context.Books.FindAsync(id);
        if (book is null)
            return;

        context.Books.Remove(book);
    }

    public async Task GlobalInsertAsync(IEnumerable<Book> books)
    {
        try
        {
            var bulkConfig = new BulkConfig { IncludeGraph = true };
            await context.BulkInsertAsync(books, bulkConfig);
        }
        catch (Exception ex)
        {
            logger.LogError("Error={ex}", ex.Message);
        }
    }
    
    public async Task BulkInsertAsync(IEnumerable<Book> books)
    {
        try
        {
            await context.BulkInsertAsync(books, b =>
            {
                // b.SetOutputIdentity = false;
                // b.PreserveInsertOrder = false;
                // b.OnSaveChangesSetFK = false;
                b.SqlBulkCopyOptions = SqlBulkCopyOptions.KeepIdentity;
            });
        }
        catch (Exception ex)
        {
            logger.LogError("Error={ex}", ex.Message);
        }
    }

    public async Task CreateManyAsync(List<Book> books)
    {
        try
        {
            logger.LogWarning("Current config AutoDetectChangesEnabled={changes}, AutoSavepointsEnabled={saves}",
                context.ChangeTracker.AutoDetectChangesEnabled, context.Database.AutoSavepointsEnabled);
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            context.Database.AutoSavepointsEnabled = false;
            var count = books.Count;
            //кажется нереальным сразу закинуть несколько сот тысяч
            //context.Books.AddRange(books);
            //await context.SaveChangesAsync();
            foreach (var book in books)
            {
                await context.Books.AddAsync(book);
                count--;
                if (count % 10000 == 0)
                {
                    logger.LogWarning("Books processed to end={count}", count);
                    await context.SaveChangesAsync();
                }
            }
            logger.LogWarning("Done");
        }
        catch (Exception ex)
        {
            logger.LogError("Error={msg}", ex.Message);
        }
        finally
        {
            context.ChangeTracker.AutoDetectChangesEnabled = true;
            context.Database.AutoSavepointsEnabled = true;
        }
    }

    public Task UpdateAsync(Book book)
    {
        context.Entry(book).State = EntityState.Modified;
        return context.SaveChangesAsync();
    }

    public Task SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }
}