using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Inventory.Dtos;
using Inventory.Application.Features.Inventory.Mappers;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Inventory.Application.Features.Inventory.Commands.AddProductToWarehouse;
public sealed class AddProductToWarehouseCommandHandler(IAppDbContext context) : IRequestHandler<AddProductToWarehouseCommand, Result<InventoryDto>>
{
    public async Task<Result<InventoryDto>> Handle(AddProductToWarehouseCommand command, CancellationToken ct)
    {
        if (!await context.Products.AnyAsync(x => x.Id == command.ProductId, ct))
            return Error.NotFound("Product.NotFound", "Product was not found.");
        if (!await context.Warehouses.AnyAsync(x => x.Id == command.WarehouseId, ct))
            return Error.NotFound("Warehouse.NotFound", "Warehouse was not found.");
        if (await context.Inventories.AnyAsync(x => x.ProductId == command.ProductId && x.WarehouseId == command.WarehouseId, ct))
            return Error.Conflict("Inventory.AlreadyExists", "The product is already available in this warehouse.");

        var result = Inventory.Domain.Inventory.Inventory.Create(Guid.NewGuid(), command.ProductId, command.WarehouseId);
        if (result.IsError) return result.Errors;
        context.Inventories.Add(result.Value);
        await context.SaveChangesAsync(ct);
        return result.Value.ToDto();
    }
}
