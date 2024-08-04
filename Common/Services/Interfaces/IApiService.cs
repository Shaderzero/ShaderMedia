using Common.Models;

namespace Common.Services.Interfaces;

public interface IApiService
{
    Task<TResult?> GetAsync<TResult>(string uri, CancellationToken cancellationToken);
    Task<TResult?> PostAsync<TMessage, TResult>(string uri, TMessage message, CancellationToken cancellationToken);
    Task<DownloadedFile> DownloadFile(string uri, CancellationToken cancellationToken = default);
}