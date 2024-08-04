using Common.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Common.Pages.Books;

public partial class InpxPage
{
    [Inject] public required IInpxService InpxService { get; set; }
    private readonly CancellationTokenSource _cts = new();
    private bool _error;

    private async Task InitInpxAsync()
    {
        _error = true;
        // var result = await InpxService.InitInpxAsync(_cts.Token);
    }
}