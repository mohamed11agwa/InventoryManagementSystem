using Inventory.Application.Features.Inventory.Commands.AddProductToWarehouse;
using Inventory.Application.Features.Inventory.Commands.AdjustStock;
using Inventory.Application.Features.Inventory.Queries.GetProductStock;
using Inventory.Application.Features.Inventory.Queries.GetRecentStockChanges;
using Inventory.Application.Features.Inventory.Queries.GetWarehouseStock;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[Route("api/inventory")]
[Authorize]
public sealed class InventoryController(ISender sender) : ApiController
{
    [HttpGet("products/{productId:guid}")]
    public async Task<IActionResult> GetProductStock(Guid productId, [FromQuery] GetProductStockQuery query, CancellationToken ct)
        => (await sender.Send(query with { ProductId = productId }, ct)).Match(Ok, Problem);

    [HttpGet("warehouses/{warehouseId:guid}")]
    public async Task<IActionResult> GetWarehouseStock(Guid warehouseId, [FromQuery] GetWarehouseStockQuery query, CancellationToken ct)
        => (await sender.Send(query with { WarehouseId = warehouseId }, ct)).Match(Ok, Problem);

    [HttpGet("changes/recent")]
    public async Task<IActionResult> GetRecentChanges([FromQuery] int count = 20, CancellationToken ct = default)
        => (await sender.Send(new GetRecentStockChangesQuery(count), ct)).Match(Ok, Problem);

    [HttpPost("warehouses")]
    [Authorize(Policy = "AdministratorOnly")]
    public async Task<IActionResult> AddProductToWarehouse(AddProductToWarehouseCommand command, CancellationToken ct)
        => (await sender.Send(command, ct)).Match(Ok, Problem);

    [HttpPost("adjustments")]
    [Authorize(Policy = "WarehouseOperatorOnly")]
    public async Task<IActionResult> AdjustStock(AdjustStockCommand command, CancellationToken ct)
        => (await sender.Send(command, ct)).Match(Ok, Problem);
}
