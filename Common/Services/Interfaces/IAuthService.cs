using Common.Models;

namespace Common.Services.Interfaces;

public interface IAuthService
{
    Task LogoutAsync(CancellationToken cancellationToken);
}