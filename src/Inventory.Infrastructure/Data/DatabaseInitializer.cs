using Inventory.Domain.Identity;
using Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Infrastructure.Data;

public sealed class DatabaseInitializer(
    ILogger<DatabaseInitializer> logger,
    AppDbContext context,
    RoleManager<IdentityRole> roleManager,
    UserManager<AppUser> userManager)
{
    public async Task InitializeAsync(CancellationToken ct = default)
    {
        try
        {
            
            await context.Database.EnsureCreatedAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    public async Task SeedAsync(CancellationToken ct = default)
    {
        try
        {
            await EnsureRolesAsync(ct);
            await EnsureUserAsync("admin@inventory.local", "Password123!", Role.Administrator, ct);
            await EnsureUserAsync("operator@inventory.local", "Password123!", Role.WarehouseOperator, ct);
            await EnsureUserAsync("manager@inventory.local", "Password123!", Role.Manager, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task EnsureRolesAsync(CancellationToken ct)
    {
        foreach (var role in Enum.GetNames<Role>())
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ", result.Errors.Select(x => x.Description)));
                }
            }
        }
    }

    private async Task EnsureUserAsync(string email, string password, Role role, CancellationToken ct)
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
            {
                throw new InvalidOperationException(
                    string.Join("; ", createResult.Errors.Select(x => x.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(user, role.ToString()))
        {
            var roleResult = await userManager.AddToRoleAsync(user, role.ToString());
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join("; ", roleResult.Errors.Select(x => x.Description)));
            }
        }
    }
}

public static class InitializerExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();

        await initializer.InitializeAsync();
        await initializer.SeedAsync();
    }
}
