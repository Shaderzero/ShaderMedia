namespace Common.Services.Interfaces;

public interface IStorage
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken);
    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken);
    Task<string?> GetStringAsync(string key, CancellationToken cancellationToken);
    Task SetStringAsync(string key, string value, CancellationToken cancellationToken);

    Task ClearAsync(CancellationToken cancellationToken);
}