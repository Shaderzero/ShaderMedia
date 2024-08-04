using Api.Controllers;
using Api.Data;
using Api.Services.Interfaces;
using Api.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Identity;

namespace Api.Services;

public class AppUserService(IUserStore<ApplicationUser> userStore, 
    UserManager<ApplicationUser> userManager, ILogger<UserController> logger) : IAppUserService
{
    public async Task<AppUser?> CreateUser(UserRegister userRegister, CancellationToken cancellationToken)
    {
        var user = Activator.CreateInstance<ApplicationUser>();
        await userStore.SetUserNameAsync(user, userRegister.Email, cancellationToken);
        var emailStore = GetEmailStore();
        await emailStore.SetEmailAsync(user, userRegister.Email, cancellationToken);
        var result = await userManager.CreateAsync(user, userRegister.Password);
        
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description);
            throw new ApiException(string.Join("; ", errors));
        }

        logger.LogInformation("User created a new account with password.");

        var newUser = await userStore.FindByNameAsync(userRegister.Email, cancellationToken);
        if (newUser is null)
        {
            throw new ApiException("Созданный пользователь не найден");
        }

        return newUser.Map();
    }

    public async Task<AppUser?> UpdateUser(AppUser user, CancellationToken cancellationToken)
    {
        var userForUpdate = await userManager.FindByIdAsync(user.Id);
        if (userForUpdate is null)
            throw new ApiException("Пользователь для обновления не найден");
        
        userForUpdate.FirstName = user.FirstName;
        userForUpdate.LastName = user.LastName;
        userForUpdate.UserName = user.UserName;
        userForUpdate.Email = user.Email;

        var result = await userManager.UpdateAsync(userForUpdate);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description);
            throw new ApiException(string.Join("; ", errors));
        }

        return userForUpdate.Map();
    }

    public async Task<AppUser?> GetUserById(string id, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
            throw new ApiException("Пользователь не найден");

        return user.Map();
    }

    public async Task<AppUser?> GetUserByName(string username, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(username);
        if (user is null)
            throw new ApiException("Пользователь не найден");

        return user.Map();
    }

    public async Task<AppUser[]?> GetUsers(CancellationToken cancellationToken)
    {
        var users = await userManager.Users.ToListAsync(cancellationToken);
        
        return users.Map().ToArray();
    }
    
    public async Task<AppUser[]?> GetUsersWithRoles(CancellationToken cancellationToken)
    {
        var users = await userManager.Users.ToListAsync(cancellationToken);
        
        var appUsers = users.Map().ToArray();
        foreach (var appUser in appUsers)
        {
            appUser.Roles = await GetUserRolesById(appUser.Id, cancellationToken);
        }

        return appUsers;
    }

    public async Task<bool> RemoveUser(AppUser user, CancellationToken cancellationToken)
    {
        var userForDelete = await userManager.FindByIdAsync(user.Id);
        if (userForDelete is null)
            throw new ApiException("Пользователь не найден");

        var result = await userManager.DeleteAsync(userForDelete);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description);
            throw new ApiException(string.Join("; ", errors));
        }

        return true;
    }

    public async Task<string[]> GetUserRolesById(string id, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
            throw new ApiException("Пользователь не найден");

        var roles = await userManager.GetRolesAsync(user);
        
        return roles.ToArray();
    }

    public async Task<string[]> SetUserRolesById(string id, string[] roles, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
            throw new ApiException("Пользователь не найден");
        
        var currentRoles = await userManager.GetRolesAsync(user);
        var rolesToDelete = currentRoles.Except(roles);
        var rolesToAdd = roles.Except(currentRoles);

        var resultDelete = await userManager.RemoveFromRolesAsync(user, rolesToDelete);
        if (!resultDelete.Succeeded)
        {
            var errors = resultDelete.Errors.Select(x => x.Description);
            throw new ApiException(string.Join("; ", errors));
        }
        
        var resultAdd = await userManager.AddToRolesAsync(user, rolesToAdd);
        if (!resultAdd.Succeeded)
        {
            var errors = resultDelete.Errors.Select(x => x.Description);
            throw new ApiException(string.Join("; ", errors));
        }
        
        var resultRoles = await userManager.GetRolesAsync(user);
        
        return resultRoles.ToArray();
    }
    
    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)userStore;
    }
}