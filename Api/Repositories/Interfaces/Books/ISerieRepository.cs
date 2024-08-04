using Api.Data.Books;

namespace Api.Repositories.Interfaces.Books;

public interface ISerieRepository
{
    Task BulkInsertAsync(IEnumerable<Series> series);
}