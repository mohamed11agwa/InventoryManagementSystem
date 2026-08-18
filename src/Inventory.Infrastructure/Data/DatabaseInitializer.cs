using Inventory.Domain.Identity;
using Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Data;

public sealed class DatabaseInitializer(
    AppDbContext context,
    RoleManager<IdentityRole> roleManager,
    UserManager<AppUser> userManager)
{
    public async Task InitializeAsync(CancellationToken ct = default)
    {
        await context.Database.EnsureCreatedAsync(ct);

        foreach (var role in Enum.GetNames<Role>())
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));
                if (!result.Succeeded)
                    throw new InvalidOperationException(string.Join("; ", result.Errors.Select(x => x.Description)));
            }
        }

        await EnsureUserAsync("admin@inventory.local", "Password123!", Role.Administrator);
        await EnsureUserAsync("operator@inventory.local", "Password123!", Role.WarehouseOperator);
        await EnsureUserAsync("manager@inventory.local", "Password123!", Role.Manager);
    }

    private async Task EnsureUserAsync(string email, string password, Role role)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new AppUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
                throw new InvalidOperationException(string.Join("; ", createResult.Errors.Select(x => x.Description)));
        }

        if (!await userManager.IsInRoleAsync(user, role.ToString()))
        {
            var roleResult = await userManager.AddToRoleAsync(user, role.ToString());
            if (!roleResult.Succeeded)
                throw new InvalidOperationException(string.Join("; ", roleResult.Errors.Select(x => x.Description)));
        }
    }
}
