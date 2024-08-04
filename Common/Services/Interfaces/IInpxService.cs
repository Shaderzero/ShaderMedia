using Shared.Models;

namespace Common.Services.Interfaces;

public interface IInpxService
{
    Task<ApiResponse<bool>> InitInpxAsync(CancellationToken cancellationToken);
}