using Inventory.Application.Features.Identity.Dtos;
using Inventory.Domain.Common.Results;

namespace Inventory.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Result<AppUserDto>> AuthenticateAsync(string email, string password);
}
