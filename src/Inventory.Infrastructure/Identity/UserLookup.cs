using Inventory.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Identity;

public sealed class UserLookup(UserManager<AppUser> userManager) : IUserLookup
{
    public async Task<Dictionary<string, string>> GetUserNamesAsync(
        IEnumerable<string> userIds,
        CancellationToken ct)
    {
        var ids = userIds.Distinct().ToList();

        return await userManager.Users
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(
                x => x.Id,
                x => x.UserName ?? x.Id,
                ct);
    }
}