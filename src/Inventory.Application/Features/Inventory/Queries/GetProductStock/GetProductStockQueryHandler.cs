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
        var inventories = await context.Inventories.AsNoTracking().Where(x => x.ProductId == query.ProductId).ToListAsync(ct);
        return inventories.Select(x => x.ToDto()).ToList();
    }
}
