using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Inventory.Dtos;
using Inventory.Application.Features.Inventory.Mappers;
using Inventory.Application.Features.Inventory.Queries.GetRecentStockChanges;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

public sealed class GetRecentStockChangesQueryHandler(IAppDbContext context, IUserLookup userLookup) : IRequestHandler<GetRecentStockChangesQuery, Result<List<StockAdjustmentDto>>>
{
    public async Task<Result<List<StockAdjustmentDto>>> Handle(GetRecentStockChangesQuery query, CancellationToken ct)
    {
        var adjustments = await context.StockAdjustments.AsNoTracking()
            .Join(
                context.Inventories,
                adjustment => adjustment.InventoryId,
                inventory => inventory.Id,
                (adjustment, inventory) => new
                {
                    Adjustment = adjustment,
                    Inventory = inventory
                })
            .Join(
                context.Products,
                x => x.Inventory.ProductId,
                product => product.Id,
                (x, product) => new
                {
                    x.Adjustment,
                    x.Inventory,
                    Product = product
                })
            .Join(
                context.Warehouses,
                x => x.Inventory.WarehouseId,
                warehouse => warehouse.Id,
                (x, warehouse) => new
                {
                    x.Adjustment,
                    x.Inventory,
                    Product = x.Product,
                    Warehouse = warehouse
                })
            .OrderByDescending(x => x.Adjustment.AdjustedAtUtc)
            .Take(query.Count)
            .ToListAsync(ct);

        var userIds = adjustments
            .Select(x => x.Adjustment.AdjustedBy);

        var userNames = await userLookup.GetUserNamesAsync(
            userIds,
            ct);

        var result = adjustments
            .Select(x => x.Adjustment.ToDto(
                x.Product.Name,
                x.Warehouse.Name,
                userNames.GetValueOrDefault(x.Adjustment.AdjustedBy, x.Adjustment.AdjustedBy)))
            .ToList();

        return result;
    }
}