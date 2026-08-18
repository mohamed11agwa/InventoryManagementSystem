namespace Inventory.Application.Common.Interfaces;

public interface IUserLookup
{
    Task<Dictionary<string, string>> GetUserNamesAsync(
        IEnumerable<string> userIds,
        CancellationToken ct);
}