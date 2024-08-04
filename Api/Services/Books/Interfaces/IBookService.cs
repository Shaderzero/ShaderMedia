using Shared.Models.Books;

namespace Api.Services.Books.Interfaces;

public interface IBookService
{
    Task<BookContainer> GetBooksAsync(BooksRequest request);
    Task<BookDto?> GetBookByIdAsync(int bookId);
}