using System.Net.Http.Json;
using Common.Models;
using Common.Services.Interfaces;

namespace Common.Services;

public class ApiService(HttpClient client) : IApiService
{
    public async Task<TResult?> GetAsync<TResult>(string uri, CancellationToken cancellationToken = default)
    {
        var response = await client.GetFromJsonAsync<TResult>(uri, cancellationToken);
        return response;
    }

    public async Task<TResult?> PostAsync<TMessage, TResult>(string uri, TMessage message, CancellationToken cancellationToken = default)
    {
        var response = await client.PostAsJsonAsync(uri, message, cancellationToken);
        var result = await response.Content.ReadFromJsonAsync<TResult>(cancellationToken);

        return result;
    }

    public async Task<DownloadedFile> DownloadFile(string uri, CancellationToken cancellationToken = default)
    {
        var file = await client.GetAsync(uri, cancellationToken);

        // var filename = file.Content.Headers.ContentDisposition!.FileNameStar; //always null
        var mediaType = file.Content.Headers.ContentType?.MediaType;
        var data = await file.Content.ReadAsStreamAsync(cancellationToken);

        return new DownloadedFile
        {
            MeidaType = mediaType,
            Data = data
        };
    }
}