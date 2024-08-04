using Api.Data.Books;

namespace Api.Repositories.Interfaces.Books;

public interface IBookRepository
{
    IQueryable<Book> GetAll();
    int GetCount();
    Task<Book?> GetByIdAsync(int id);
    Task<Book?> GetByTitleAsync(string title);
    Task<Book> CreateAsync(Book author);
    Task DeleteAsync(int id);
    Task UpdateAsync(Book book);
    Task SaveChangesAsync();
    Task CreateManyAsync(List<Book>  books);
    Task BulkInsertAsync(IEnumerable<Book> books);
    Task GlobalInsertAsync(IEnumerable<Book> books);
}