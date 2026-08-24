using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Inventory.Dtos;
using Inventory.Application.Features.Inventory.Mappers;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Inventory.Application.Features.Inventory.Commands.AdjustStock;
public sealed class AdjustStockCommandHandler(IAppDbContext context, IUser user) : IRequestHandler<AdjustStockCommand, Result<InventoryDto>>
{
    public async Task<Result<InventoryDto>> Handle(AdjustStockCommand command, CancellationToken ct)
    {
        var inventory = await context.Inventories.FirstOrDefaultAsync(
            x => x.ProductId == command.ProductId && x.WarehouseId == command.WarehouseId, ct);
        if (inventory is null)
            return Error.NotFound("Inventory.NotFound", "The product is not available in this warehouse.");

        if (string.IsNullOrWhiteSpace(user.Id))
            return Error.Unauthorized("User.Unauthenticated", "An authenticated user is required.");

        var result = inventory.AdjustStock(command.QuantityChange, command.Reason, user.Id, DateTimeOffset.UtcNow);

        if (result.IsError)
            return result.Errors;

        try
        {
            await context.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(
                "Inventory.ConcurrencyConflict",
                "The inventory was modified by another user. Please reload the current stock and try again.");
        }

        var productName = await context.Products
    .Where(x => x.Id == inventory.ProductId)
    .Select(x => x.Name)
    .FirstAsync(ct);

        var warehouseName = await context.Warehouses
            .Where(x => x.Id == inventory.WarehouseId)
            .Select(x => x.Name)
            .FirstAsync(ct);

        return inventory.ToDto(productName, warehouseName);
    }
}
