using Inventory.Application.Features.Identity.Dtos;
using Inventory.Domain.Common.Results;

namespace Inventory.Application.Common.Interfaces;

public interface ITokenProvider
{
    Task<Result<TokenResponse>> GenerateJwtTokenAsync(AppUserDto user, CancellationToken ct = default);
}
