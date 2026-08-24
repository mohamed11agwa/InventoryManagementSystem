using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Inventory.Dtos;
using Inventory.Application.Features.Inventory.Mappers;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Inventory.Application.Features.Inventory.Queries.GetProductStock;
public sealed class GetProductStockQueryHandler(IAppDbContext context) : IRequestHandler<GetProductStockQuery, Result<List<InventoryDto>>>
{
    public async Task<Result<List<InventoryDto>>> Handle(GetProductStockQuery query, CancellationToken ct)
    {
        var inventories = await context.Inventories
    .AsNoTracking()
    .Where(x => x.ProductId == query.ProductId)
    .Join(
        context.Products,
        inventory => inventory.ProductId,
        product => product.Id,
        (inventory, product) => new
        {
            Inventory = inventory,
            ProductName = product.Name
        })
    .Join(
        context.Warehouses,
        x => x.Inventory.WarehouseId,
        warehouse => warehouse.Id,
        (x, warehouse) => new
        {
            x.Inventory,
            x.ProductName,
            WarehouseName = warehouse.Name
        })
    .ToListAsync(ct);

        return inventories
            .Select(x => x.Inventory.ToDto(x.ProductName, x.WarehouseName))
            .ToList();
    }
}
