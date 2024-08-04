using Api.Data;
using Api.Data.Books;
using Api.Repositories.Interfaces.Books;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Books;

public class AuthorRepository(BookDbContext context, ILogger<AuthorRepository> logger) : IAuthorRepository
{
    public IQueryable<Author> GetAll()
    {
        return context.Authors;
    }

    public Task<Author?> GetByIdAsync(int id)
    {
        return context.Authors.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<Author?> GetByNameAsync(string? lastName, string? firstName, string? middleName = null)
    {
        var authorsQuery =
            context.Authors.Where(x => x.LastName != null && x.LastName.Equals(lastName));

        if (!string.IsNullOrEmpty(firstName))
        {
            authorsQuery = authorsQuery.Where(x => x.FirstName != null && x.FirstName.Equals(firstName));
        }

        if (!string.IsNullOrEmpty(middleName))
        {
            authorsQuery = authorsQuery.Where(x => x.MiddleName != null && x.MiddleName.Equals(middleName));
        }

        return authorsQuery.FirstOrDefaultAsync();
    }

    public async Task<Author> GetOrCreateAsync(Author author)
    {
        
        var existedAuthor = await GetByNameAsync(author.LastName, author.FirstName, author.MiddleName);
        if (existedAuthor is not null)
            return existedAuthor;
        
        return await CreateAsync(author);
    }

    public async Task<Author> CreateAsync(Author author)
    {
        var existedAuthor = await GetByNameAsync(author.LastName, author.FirstName, author.MiddleName);
        if (existedAuthor is not null)
            return existedAuthor;

        var newAuthor = await context.Authors.AddAsync(author);
        
        return newAuthor.Entity;
    }

    public async Task DeleteAsync(int id)
    {
        var author = await context.Authors.FindAsync(id);
        if (author is null)
            return;

        context.Authors.Remove(author);
    }

    public Task BulkInsertAsync(IEnumerable<Author> authors)
    {
        return context.BulkInsertAsync(authors, b =>
        {
            // b.SetOutputIdentity = false;
            // b.PreserveInsertOrder = false;
            // b.OnSaveChangesSetFK = false;
            b.SqlBulkCopyOptions = SqlBulkCopyOptions.KeepIdentity;
        });
    }

    public Task BulkInsertAsync(IEnumerable<BookAuthor> bookAuthors)
    {
        return context.BulkInsertAsync(bookAuthors, b =>
        {
            // b.SetOutputIdentity = false;
            // b.PreserveInsertOrder = false;
            // b.OnSaveChangesSetFK = false;
            b.SqlBulkCopyOptions = SqlBulkCopyOptions.KeepIdentity;
        });
    }

    public Task UpdateAsync(Author author)
    {
        context.Entry(author).State = EntityState.Modified;
        return context.SaveChangesAsync();
    }

    public Task SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }
}