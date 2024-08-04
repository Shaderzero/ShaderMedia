using Api.Data;
using Api.Data.Books;
using Api.Repositories.Interfaces.Books;
using EFCore.BulkExtensions;

namespace Api.Repositories.Books;

public class KeywordRepository(BookDbContext context) : IKeywordRepository
{
    public Task BulkInsertAsync(IEnumerable<Keyword> keywords)
    {
        return context.BulkInsertAsync(keywords, b =>
        {
            // b.SetOutputIdentity = false;
            // b.PreserveInsertOrder = false;
            // b.OnSaveChangesSetFK = false;
            b.SqlBulkCopyOptions = SqlBulkCopyOptions.KeepIdentity;
        });
    }

    public Task BulkInsertAsync(IEnumerable<BookKeyword> bookKeywords)
    {
        return context.BulkInsertAsync(bookKeywords, b =>
        {
            // b.SetOutputIdentity = false;
            // b.PreserveInsertOrder = false;
            // b.OnSaveChangesSetFK = false;
            b.SqlBulkCopyOptions = SqlBulkCopyOptions.KeepIdentity;
        });
    }
}