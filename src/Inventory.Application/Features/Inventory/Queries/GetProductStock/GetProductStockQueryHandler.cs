using Inventory.Application.Common.Interfaces;
using Inventory.Application.Common.Models;
using Inventory.Application.Features.Inventory.Dtos;
using Inventory.Application.Features.Inventory.Mappers;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Inventory.Queries.GetProductStock;

public sealed class GetProductStockQueryHandler(IAppDbContext context)
    : IRequestHandler<GetProductStockQuery, Result<PagedResult<InventoryDto>>>
{
    public async Task<Result<PagedResult<InventoryDto>>> Handle(GetProductStockQuery query, CancellationToken ct)
    {
        var inventories = context.Inventories.AsNoTracking().Where(x => x.ProductId == query.ProductId)
            .Join(context.Products, x => x.ProductId, p => p.Id, (x, p) => new { Inventory = x, ProductName = p.Name })
            .Join(context.Warehouses, x => x.Inventory.WarehouseId, w => w.Id, (x, w) => new { x.Inventory, x.ProductName, WarehouseName = w.Name });

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            inventories = inventories.Where(x => x.WarehouseName.Contains(search));
        }

        inventories = query.SortBy.ToLowerInvariant() switch
        {
            "quantity" => query.Descending ? inventories.OrderByDescending(x => x.Inventory.Quantity) : inventories.OrderBy(x => x.Inventory.Quantity),
            _ => query.Descending ? inventories.OrderByDescending(x => x.WarehouseName) : inventories.OrderBy(x => x.WarehouseName)
        };

        var totalCount = await inventories.CountAsync(ct);
        var page = await inventories.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync(ct);
        var items = page.Select(x => x.Inventory.ToDto(x.ProductName, x.WarehouseName)).ToList();

        return new PagedResult<InventoryDto>(items, query.Page, query.PageSize, totalCount);
    }
}
