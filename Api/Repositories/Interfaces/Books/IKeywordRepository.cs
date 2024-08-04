using Api.Data.Books;

namespace Api.Repositories.Interfaces.Books;

public interface IKeywordRepository
{
    Task BulkInsertAsync(IEnumerable<Keyword> keywords);
    Task BulkInsertAsync(IEnumerable<BookKeyword> bookKeywords);
}