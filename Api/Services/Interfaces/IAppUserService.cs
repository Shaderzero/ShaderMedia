using Shared.Models.Identity;

namespace Api.Services.Interfaces;

public interface IAppUserService
{
    Task<AppUser?> CreateUser(UserRegister userRegister, CancellationToken cancellationToken);
    Task<AppUser?> UpdateUser(AppUser user, CancellationToken cancellationToken);
    Task<AppUser?> GetUserById(string id, CancellationToken cancellationToken);
    Task<AppUser?> GetUserByName(string username, CancellationToken cancellationToken);
    Task<AppUser[]?> GetUsers(CancellationToken cancellationToken);
    Task<AppUser[]?> GetUsersWithRoles(CancellationToken cancellationToken);
    Task<bool> RemoveUser(AppUser user, CancellationToken cancellationToken);

    Task<string[]> GetUserRolesById(string id, CancellationToken cancellationToken);
    Task<string[]> SetUserRolesById(string id, string[] roles, CancellationToken cancellationToken);
}