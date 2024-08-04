using Api.Data;
using Shared.Models.Identity;

namespace Api.Utils;

public static class UserExtensions
{
    public static AppUser Map(this ApplicationUser user)
    {
        return new AppUser
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName
        };
    }

    public static IEnumerable<AppUser> Map(this IEnumerable<ApplicationUser> users)
    {
        return users.Select(user => user.Map());
    }
}