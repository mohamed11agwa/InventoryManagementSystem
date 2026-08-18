using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Inventory.Dtos;
using Inventory.Application.Features.Inventory.Queries.GetRecentStockChanges;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

public sealed class GetRecentStockChangesQueryHandler(
    IAppDbContext context,
    IUserLookup userLookup)
    : IRequestHandler<
        GetRecentStockChangesQuery,
        Result<List<StockAdjustmentDto>>>
{
    public async Task<Result<List<StockAdjustmentDto>>> Handle(
        GetRecentStockChangesQuery query,
        CancellationToken ct)
    {
        var adjustments = await context.StockAdjustments
            .AsNoTracking()
            .Join(
                context.Inventories,
                a => a.InventoryId,
                i => i.Id,
                (a, i) => new { Adjustment = a, Inventory = i })
            .Join(
                context.Products,
                x => x.Inventory.ProductId,
                p => p.Id,
                (x, p) => new { x.Adjustment, x.Inventory, Product = p })
            .Join(
                context.Warehouses,
                x => x.Inventory.WarehouseId,
                w => w.Id,
                (x, w) => new
                {
                    x.Adjustment,
                    ProductName = x.Product.Name,
                    WarehouseName = w.Name
                })
            .OrderByDescending(x => x.Adjustment.AdjustedAtUtc)
            .Take(query.Count)
            .ToListAsync(ct);

        var userIds = adjustments
            .Select(x => x.Adjustment.AdjustedBy);

        var userNames = await userLookup.GetUserNamesAsync(userIds, ct);

        var result = adjustments.Select(x =>
            new StockAdjustmentDto(
                x.Adjustment.Id,
                x.ProductName,
                x.WarehouseName,
                x.Adjustment.QuantityChange,
                x.Adjustment.PreviousQuantity,
                x.Adjustment.NewQuantity,
                x.Adjustment.AdjustedAtUtc,
                userNames.GetValueOrDefault(
                    x.Adjustment.AdjustedBy,
                    x.Adjustment.AdjustedBy),
                x.Adjustment.Reason))
            .ToList();

        return result;
    }
}