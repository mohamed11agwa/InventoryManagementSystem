using Inventory.Application.Features.Warehouses.Commands.CreateWarehouse;
using Inventory.Application.Features.Warehouses.Commands.UpdateWarehouse;
using Inventory.Application.Features.Warehouses.Queries.GetWarehouses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[Route("api/warehouses")]
[Authorize]
public sealed class WarehousesController(ISender sender) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        return (await sender.Send(new GetWarehousesQuery(), ct)).Match(Ok, Problem);
    }


    [HttpPost]
    [Authorize(Policy = "AdministratorOnly")]
    public async Task<IActionResult> Create(CreateWarehouseCommand command, CancellationToken ct)
    {
        return (await sender.Send(command, ct)).Match(Ok, Problem);
    }
       

    [HttpPut("{warehouseId:guid}")]
    [Authorize(Policy = "AdministratorOnly")]
    public async Task<IActionResult> Update(Guid warehouseId, UpdateWarehouseCommand command, CancellationToken ct)
    {
        var updateCommand = new UpdateWarehouseCommand(warehouseId, command.Name);

        return (await sender.Send(updateCommand, ct)).Match(Ok, Problem);
    }

}
