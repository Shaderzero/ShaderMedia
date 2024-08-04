using Common.Services.Interfaces;
using Shared;
using Shared.Models;

namespace Common.Services;

public class InpxService(IApiService apiService) : IInpxService
{
    public async Task<ApiResponse<bool>> InitInpxAsync(CancellationToken cancellationToken)
    {
        var result =
            await apiService.GetAsync<ApiResponse<bool>>(ApiPaths.InpxInit, cancellationToken);

        return result;
    }
}