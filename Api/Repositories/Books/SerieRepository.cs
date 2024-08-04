using Api.Data;
using Api.Data.Books;
using Api.Repositories.Interfaces.Books;
using EFCore.BulkExtensions;

namespace Api.Repositories.Books;

public class SerieRepository(BookDbContext context) : ISerieRepository
{
    public Task BulkInsertAsync(IEnumerable<Series> series)
    {
        return context.BulkInsertAsync(series, b =>
        {
            // b.SetOutputIdentity = false;
            // b.PreserveInsertOrder = false;
            // b.OnSaveChangesSetFK = false;
            b.SqlBulkCopyOptions = SqlBulkCopyOptions.KeepIdentity;
        });
    }
}