using Api.Data.Books;

namespace Api.Repositories.Interfaces.Books;

public interface IGenreRepository
{
    Task BulkInsertAsync(IEnumerable<Genre> genres);
    Task BulkInsertAsync(IEnumerable<BookGenre> bookGenres);
}