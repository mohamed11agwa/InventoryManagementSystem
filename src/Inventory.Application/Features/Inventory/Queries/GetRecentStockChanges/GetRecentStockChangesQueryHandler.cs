using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Inventory.Dtos;
using Inventory.Application.Features.Inventory.Mappers;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Inventory.Application.Features.Inventory.Queries.GetRecentStockChanges;
public sealed class GetRecentStockChangesQueryHandler(IAppDbContext context) : IRequestHandler<GetRecentStockChangesQuery, Result<List<StockAdjustmentDto>>>
{
    public async Task<Result<List<StockAdjustmentDto>>> Handle(GetRecentStockChangesQuery query, CancellationToken ct)
    {
        var adjustments = await context.StockAdjustments
            .AsNoTracking()
            .Join(context.Inventories, a => a.InventoryId, i => i.Id, (a, i) => new { Adjustment = a, Inventory = i })
            .OrderByDescending(x => x.Adjustment.AdjustedAtUtc)
            .Take(query.Count)
            .ToListAsync(ct);

        return adjustments.Select(x => x.Adjustment.ToDto(x.Inventory.ProductId, x.Inventory.WarehouseId)).ToList();
    }
}
