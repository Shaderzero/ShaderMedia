using Api.Data.Books;

namespace Api.Repositories.Interfaces.Books;

public interface IAuthorRepository
{
    IQueryable<Author> GetAll();
    Task<Author?> GetByIdAsync(int id);
    Task<Author?> GetByNameAsync(string lastName, string? firstName, string? middleName = null);
    Task<Author> CreateAsync(Author author);
    Task DeleteAsync(int id);
    Task UpdateAsync(Author author);
    Task SaveChangesAsync();
    Task BulkInsertAsync(IEnumerable<Author> authors);
    Task BulkInsertAsync(IEnumerable<BookAuthor> bookAuthors);
}