using Common.Services.Books.Interfaces;
using Common.Services.Interfaces;
using Common.Utils;
using Microsoft.JSInterop;
using Shared;
using Shared.Models;
using Shared.Models.Books;
using Shared.Utils;

namespace Common.Services.Books;

public class BookService(IApiService apiService, IJSRuntime js) : IBookService
{
    public Task GetBooks()
    {
        throw new NotImplementedException();
    }

    public async Task<BookContainer> GetBooksAsync(BooksRequest request, CancellationToken cancellationToken)
    {
        var result =
            await apiService.PostAsync<BooksRequest, ApiResponse<BookContainer>>(ApiPaths.Books, request,
                cancellationToken);
        return result?.Value ?? new BookContainer();
    }

    public async Task DownloadBooksAsync(BookDto book, CancellationToken cancellationToken)
    {
        var bookId = book.Id;
        var url = RouteHelper.ReplaceRouteParameter(ApiPaths.BookDownload, nameof(bookId), bookId.ToString());
        var result = await apiService.DownloadFile(url, cancellationToken);

        using var memoryStream = new MemoryStream();
        await result.Data.CopyToAsync(memoryStream, cancellationToken);
        var bytes = memoryStream.ToArray();
        
        var content = Convert.ToBase64String(bytes);
        var filename = FilenameUtils.GetBookSafeName(book);

        await js.InvokeVoidAsync("jsSaveAsFile", cancellationToken, filename, content);
    }

    public async Task<string> DownloadBookContentAsync(BookDto book, CancellationToken cancellationToken)
    {
        var bookId = book.Id;
        var url = RouteHelper.ReplaceRouteParameter(ApiPaths.BookDownload, nameof(bookId), bookId.ToString());
        var result = await apiService.DownloadFile(url, cancellationToken);

        // using var memoryStream = new MemoryStream();
        // await result.Data.CopyToAsync(memoryStream, cancellationToken);
        var body = await Fb2Service.GetBody(book, result.Data, cancellationToken);

        return body;
    }

    public async Task<BookDto?> GetBookInfoAsync(int bookId, CancellationToken cancellationToken)
    {
        var url = RouteHelper.ReplaceRouteParameter(ApiPaths.BookInfo, nameof(bookId), bookId.ToString());
        var result = await apiService.GetAsync<BookDto>(url, cancellationToken);

        return result;
    }
}