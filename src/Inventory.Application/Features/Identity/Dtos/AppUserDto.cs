namespace Inventory.Application.Features.Identity.Dtos;

public sealed record AppUserDto(string UserId, string Email, IList<string> Roles);
