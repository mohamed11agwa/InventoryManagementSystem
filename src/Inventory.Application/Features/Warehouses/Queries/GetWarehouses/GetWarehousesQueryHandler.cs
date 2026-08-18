using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Warehouses.Dtos;
using Inventory.Application.Features.Warehouses.Mappers;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Inventory.Application.Features.Warehouses.Queries.GetWarehouses;
public sealed class GetWarehousesQueryHandler(IAppDbContext context) : IRequestHandler<GetWarehousesQuery, Result<List<WarehouseDto>>>
{
    public async Task<Result<List<WarehouseDto>>> Handle(GetWarehousesQuery query, CancellationToken ct)
    {
        var warehouses = await context.Warehouses.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct);
        return warehouses.Select(x => x.ToDto()).ToList();
    }
}
