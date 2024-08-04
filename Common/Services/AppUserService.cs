using Common.Services.Interfaces;
using Shared;
using Shared.Models;
using Shared.Models.Identity;

namespace Common.Services;

public class AppUserService(IApiService apiService) : IAppUserService
{
    public async Task<ApiResponse<AppUser>?> CreateUser(UserRegister user, CancellationToken cancellationToken)
    {
        var result =
            await apiService.PostAsync<UserRegister, ApiResponse<AppUser>>(ApiPaths.UserRegister, user, cancellationToken);

        return result;
    }

    public Task<ApiResponse<AppUser>> UpdateUser(AppUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<AppUser>> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<AppUser>> GetUserByName(string username, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse<AppUser[]>?> GetUsers(CancellationToken cancellationToken)
    {
        var result =
            await apiService.GetAsync<ApiResponse<AppUser[]>>(ApiPaths.Users, cancellationToken);

        return result;
    }

    public Task<ApiResponse<bool>> RemoveUser(AppUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}