using Inventory.Application.Features.Identity.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;

namespace Inventory.Application.Features.Identity.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<TokenResponse>>;
