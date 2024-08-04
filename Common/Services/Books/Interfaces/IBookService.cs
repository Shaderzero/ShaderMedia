using Shared.Models.Books;

namespace Common.Services.Books.Interfaces;

public interface IBookService
{
    Task GetBooks();
    Task<BookContainer> GetBooksAsync(BooksRequest request, CancellationToken cancellationToken);
    Task DownloadBooksAsync(BookDto book, CancellationToken cancellationToken);
    Task<BookDto?> GetBookInfoAsync(int bookId, CancellationToken cancellationToken);
    Task<string> DownloadBookContentAsync(BookDto book, CancellationToken cancellationToken);
}