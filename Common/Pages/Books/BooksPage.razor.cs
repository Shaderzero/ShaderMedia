using System.Text;
using Common.Services.Books.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Shared.Dictionaries;
using Shared.Models;
using Shared.Models.Books;

namespace Common.Pages.Books;

public partial class BooksPage : ComponentBase, IDisposable
{
    [Inject] public required IBookService BookService { get; set; }
    [Inject] public required ILogger<BooksPage> Logger { get; set; }
    private readonly CancellationTokenSource _cts = new();
    private readonly BooksRequest _bookRequest = new();
    private EditContext? _editContext;
    private List<BookDto>? _books;
    private BookDto? _selectedBook;

    protected override Task OnInitializedAsync()
    {
        _editContext = new EditContext(_bookRequest);
        return GetBooksAsync();
    }

    private Task HandleOnValidSubmitAsync()
    {
        return GetBooksAsync();
    }

    private Task GoPage(Pagination pagination)
    {
        _bookRequest.Pagination = pagination;
        return GetBooksAsync();
    }

    private async Task GetBooksAsync()
    {
        var bookContainer = await BookService.GetBooksAsync(_bookRequest, _cts.Token);
        _books = bookContainer.Books;
        _bookRequest.Pagination = bookContainer.Pagination;
    }

    private static string GenerateSeriesWithNumber(BookDto book)
    {
        var sb = new StringBuilder();
        if (!string.IsNullOrEmpty(book.Series?.Name))
        {
            sb.Append(book.Series?.Name);
        }
        else return string.Empty;

        if (book.SeriesNo > 0)
        {
            sb.Append($" ({book.SeriesNo})");
        }

        return sb.ToString();
    }

    private void SelectBook(BookDto book)
    {
        if (_selectedBook == book)
        {
            _selectedBook = null;
        }
        else
        {
            _selectedBook = book;
        }
    }

    private Task SetSortingAsync(BookSortColumn column)
    {
        if (_bookRequest.SortColumn == column)
            _bookRequest.SortDirection =
                _bookRequest.SortDirection == SortDirection.Down ? SortDirection.Up : SortDirection.Down;
        else
        {
            _bookRequest.SortColumn = column;
            _bookRequest.SortDirection = SortDirection.Down;
        }

        return GetBooksAsync();
    }

    private SortDirection? GetSortDirection(BookSortColumn column)
    {
        return _bookRequest.SortColumn != column ? SortDirection.None : _bookRequest.SortDirection;
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

    public Task DownloadBook()
    {
        if (_selectedBook is null)
            return Task.CompletedTask;
        
        Logger.LogInformation("Запрошено скачивание книги {name}, zip {zip}, файл {file}", _selectedBook.Title, _selectedBook.ZipName, _selectedBook.File + ".fb2");
        return BookService.DownloadBooksAsync(_selectedBook, _cts.Token);
    }
}