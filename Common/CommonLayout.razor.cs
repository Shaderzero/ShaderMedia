using Common.Services;
using Microsoft.AspNetCore.Components;

namespace Common;

public partial class CommonLayout
{
    [Parameter]
    public required Type Anchor { get; set; }
    [Inject]
    public required NavigationManager NavigationManager { get; set; }
    [Inject]
    public required RefreshService RefreshService { get; set; }

    private readonly CancellationTokenSource _cts = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            RefreshService.Init(NavigationManager);
        }
    }
}