using Common.Services.Books.Interfaces;
using Microsoft.AspNetCore.Components;
using Shared.Models.Books;

namespace Common.Components;

public partial class BookInfoComponent : ComponentBase, IDisposable
{
    [Parameter] public required BookDto Book { get; set; }
    [Parameter] public EventCallback OnDownload { get; set; }
    [Inject] public required IBookService BookService { get; set; }
    private string? _bookAuthors;
    private string? _bookGenres;
    private string? _genreLabel;
    private BookDto? _book;
    private readonly CancellationTokenSource _cts = new();
    private bool _disposed;
    

    protected override async Task OnInitializedAsync()
    {
        _book = await BookService.GetBookInfoAsync(Book.Id, _cts.Token) ?? Book;
        _genreLabel = Book.Genres.Count > 1 ? "Жанры" : "Жанр";
        _bookAuthors = string.Join(", ", Book.Authors.GenerateName());
        _bookGenres = string.Join(", ", Book.Genres.GenerateName());
    }

    private Task Download()
    {
        return OnDownload.InvokeAsync();
    }

    private string CoverDataSrc()
    {
        if (_book?.Cover?.Data is not null)
        {
            var base64 = Convert.ToBase64String(_book.Cover.Data);
            return $"data:{_book.Cover.ContentType};base64,{base64}";
        }

        return string.Empty;
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _cts.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}