namespace Inventory.Application.Features.Identity.Dtos;

public sealed record TokenResponse(string AccessToken, DateTime ExpiresOnUtc);
