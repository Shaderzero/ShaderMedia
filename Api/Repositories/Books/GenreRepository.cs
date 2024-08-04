using Api.Data;
using Api.Data.Books;
using Api.Repositories.Interfaces.Books;
using EFCore.BulkExtensions;

namespace Api.Repositories.Books;

public class GenreRepository(BookDbContext context) : IGenreRepository
{
    public Task BulkInsertAsync(IEnumerable<Genre> genres)
    {
        return context.BulkInsertAsync(genres, b =>
        {
            // b.SetOutputIdentity = false;
            // b.PreserveInsertOrder = false;
            // b.OnSaveChangesSetFK = false;
            b.SqlBulkCopyOptions = SqlBulkCopyOptions.KeepIdentity;
        });
    }

    public Task BulkInsertAsync(IEnumerable<BookGenre> bookGenres)
    {
        return context.BulkInsertAsync(bookGenres, b =>
        {
            // b.SetOutputIdentity = false;
            // b.PreserveInsertOrder = false;
            // b.OnSaveChangesSetFK = false;
            b.SqlBulkCopyOptions = SqlBulkCopyOptions.KeepIdentity;
        });
    }
}