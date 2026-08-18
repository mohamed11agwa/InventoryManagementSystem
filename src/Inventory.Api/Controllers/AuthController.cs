using Inventory.Application.Features.Identity.Commands.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[Route("api/auth")]
public sealed class AuthController(ISender sender) : ApiController
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command, CancellationToken ct)
    {
        var result = await sender.Send(command, ct);
        return result.Match(response => Ok(response), Problem);
    }
}
