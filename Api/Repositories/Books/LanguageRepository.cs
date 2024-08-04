using Api.Data;
using Api.Data.Books;
using Api.Repositories.Interfaces.Books;
using EFCore.BulkExtensions;

namespace Api.Repositories.Books;

public class LanguageRepository(BookDbContext context) : ILanguageRepository
{
    public Task BulkInsertAsync(IEnumerable<Language> languages)
    {
        return context.BulkInsertAsync(languages, b =>
        {
            // b.SetOutputIdentity = false;
            // b.PreserveInsertOrder = false;
            // b.OnSaveChangesSetFK = false;
            b.SqlBulkCopyOptions = SqlBulkCopyOptions.KeepIdentity;
        });
    }
}