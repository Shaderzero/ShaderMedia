using Common.Services.Books.Interfaces;
using Microsoft.AspNetCore.Components;
using Shared.Models.Books;

namespace Common.Pages.Books;

public partial class ReaderPage : ComponentBase, IDisposable
{
    [Parameter] public required int BookId { get; set; }
    [Inject] public required IBookService BookService { get; set; }
    private BookDto? _book;
    private string _content = string.Empty;
    private readonly CancellationTokenSource _cts = new();
    private bool _disposed;

    protected override async Task OnParametersSetAsync()
    {
        _book = await BookService.GetBookInfoAsync(BookId, _cts.Token);
        await GetContent();
    }

    private async Task GetContent()
    {
        if (_book is null)
            return;
        
        var bookContent = await BookService.DownloadBookContentAsync(_book, _cts.Token);

        _content = bookContent;
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _content = string.Empty;
        _cts.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}