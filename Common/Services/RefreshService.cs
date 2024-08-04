using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Common.Services;

public class RefreshService : IDisposable
{
    private NavigationManager? _navigationManager;
    private Func<Task>? _refreshTask;

    public void Init(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public void Enable(Func<Task> refreshTask) => _refreshTask = refreshTask;

    public void Disable() => _refreshTask = null;

    public void Refresh() => _refreshTask?.Invoke();

    private void LocationRefresh(object? sender, LocationChangedEventArgs e) => Disable();

    public void Dispose()
    {
        if (_navigationManager != null)
            _navigationManager.LocationChanged -= LocationRefresh;

        _navigationManager = null;
    }
}
