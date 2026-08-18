using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Identity.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;

namespace Inventory.Application.Features.Identity.Commands.Login;

public sealed class LoginCommandHandler(IIdentityService identityService, ITokenProvider tokenProvider)
    : IRequestHandler<LoginCommand, Result<TokenResponse>>
{
    public async Task<Result<TokenResponse>> Handle(LoginCommand command, CancellationToken ct)
    {
        var userResult = await identityService.AuthenticateAsync(command.Email, command.Password);
        if (userResult.IsError) return userResult.Errors;

        var tokenResult = await tokenProvider.GenerateJwtTokenAsync(userResult.Value, ct);
        return tokenResult.IsError ? tokenResult.Errors : tokenResult.Value;
    }
}
