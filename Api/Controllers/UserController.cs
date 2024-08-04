using Api.Data;
using Api.Services.Interfaces;
using Api.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Models;
using Shared.Models.Identity;

namespace Api.Controllers;

[ApiController]
public class UserController(IAppUserService appUserService) : ControllerBase
{
    private IEnumerable<IdentityError>? _identityErrors;

    [HttpPost]
    [Route(ApiPaths.UserRegister)]
    public async Task<ApiResponse<AppUser>> CreateAsync(UserRegister userRegister)
    {
        try
        {
            var newUser = await appUserService.CreateUser(userRegister, HttpContext.RequestAborted);
            return new ApiResponse<AppUser>
            {
                Value = newUser
            };
        }
        catch (ApiException ex)
        {
            return new ApiResponse<AppUser>
            {
                Errors = ex.Message.Split("; ")
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<AppUser>
            {
                Errors = [ex.Message]
            };
        }
    }
}