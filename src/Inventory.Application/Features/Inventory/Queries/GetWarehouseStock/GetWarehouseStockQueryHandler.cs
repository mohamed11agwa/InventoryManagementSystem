using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Inventory.Dtos;
using Inventory.Application.Features.Inventory.Mappers;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Inventory.Application.Features.Inventory.Queries.GetWarehouseStock;
public sealed class GetWarehouseStockQueryHandler(IAppDbContext context) : IRequestHandler<GetWarehouseStockQuery, Result<List<InventoryDto>>>
{
    public async Task<Result<List<InventoryDto>>> Handle(GetWarehouseStockQuery query, CancellationToken ct)
    {
        var inventories = await context.Inventories.AsNoTracking().Where(x => x.WarehouseId == query.WarehouseId).ToListAsync(ct);
        return inventories.Select(x => x.ToDto()).ToList();
    }
}
