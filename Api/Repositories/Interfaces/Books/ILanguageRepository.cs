using Api.Data.Books;

namespace Api.Repositories.Interfaces.Books;

public interface ILanguageRepository
{
    Task BulkInsertAsync(IEnumerable<Language> languages);
}