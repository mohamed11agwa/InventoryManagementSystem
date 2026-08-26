using Inventory.Application.Features.Customers.Commands.CreateCustomer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[Route("api/customers")]
[Authorize]
public sealed class CustomersController(ISender sender) : ApiController
{
    [HttpPost]
    [Authorize(Policy = "SalesOrderManagement")]
    public async Task<IActionResult> Create(CreateCustomerCommand command, CancellationToken ct)
        => (await sender.Send(command, ct)).Match(Ok, Problem);
}
