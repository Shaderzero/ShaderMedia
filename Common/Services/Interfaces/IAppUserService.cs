using Shared.Models;
using Shared.Models.Identity;

namespace Common.Services.Interfaces;

public interface IAppUserService
{
    Task<ApiResponse<AppUser>?> CreateUser(UserRegister user, CancellationToken cancellationToken);
    Task<ApiResponse<AppUser>?> UpdateUser(AppUser user, CancellationToken cancellationToken);
    Task<ApiResponse<AppUser>?> GetUserById(Guid id, CancellationToken cancellationToken);
    Task<ApiResponse<AppUser>?> GetUserByName(string username, CancellationToken cancellationToken);
    Task<ApiResponse<AppUser[]>?> GetUsers(CancellationToken cancellationToken);
    Task<ApiResponse<bool>?> RemoveUser(AppUser user, CancellationToken cancellationToken);
}