using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Identity.Dtos;
using Inventory.Domain.Common.Results;
using Microsoft.AspNetCore.Identity;

namespace Inventory.Infrastructure.Identity;

public sealed class IdentityService(UserManager<AppUser> userManager) : IIdentityService
{
    public async Task<Result<AppUserDto>> AuthenticateAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Error.Conflict("Identity.InvalidCredentials", "Email or password is incorrect.");

        if (!user.EmailConfirmed || !await userManager.CheckPasswordAsync(user, password))
            return Error.Conflict("Identity.InvalidCredentials", "Email or password is incorrect.");

        return new AppUserDto(user.Id, user.Email!, await userManager.GetRolesAsync(user));
    }
}
