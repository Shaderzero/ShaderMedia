using Blazored.LocalStorage;
using Common.Services.Interfaces;

namespace Web.Services;

public class WebStorage(ILocalStorageService localStorage) : IStorage
{
    public Task ClearAsync(CancellationToken cancellationToken)
    {
        return localStorage.ClearAsync(cancellationToken).AsTask();
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        var value = await localStorage.GetItemAsync<T>(key, cancellationToken);
        return value;
    }

    public Task SetAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        return localStorage.SetItemAsync(key, value, cancellationToken).AsTask();
    }

    public Task<string?> GetStringAsync(string key, CancellationToken cancellationToken)
    {
        return localStorage.GetItemAsStringAsync(key, cancellationToken).AsTask();
    }

    public Task SetStringAsync(string key, string value, CancellationToken cancellationToken)
    {
        return localStorage.SetItemAsStringAsync(key, value, cancellationToken).AsTask();
    }
}