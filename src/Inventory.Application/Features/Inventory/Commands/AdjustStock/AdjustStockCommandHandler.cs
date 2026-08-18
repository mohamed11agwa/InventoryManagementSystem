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
        if (result.IsError) return result.Errors;
        await context.SaveChangesAsync(ct);
        return inventory.ToDto();
    }
}
