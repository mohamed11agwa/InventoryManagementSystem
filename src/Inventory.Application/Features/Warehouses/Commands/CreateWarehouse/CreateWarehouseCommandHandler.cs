using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Warehouses.Dtos;
using Inventory.Application.Features.Warehouses.Mappers;
using Inventory.Domain.Common.Results;
using Inventory.Domain.Warehouses;
using MediatR;
namespace Inventory.Application.Features.Warehouses.Commands.CreateWarehouse;
public sealed class CreateWarehouseCommandHandler(IAppDbContext context) : IRequestHandler<CreateWarehouseCommand, Result<WarehouseDto>>
{
    public async Task<Result<WarehouseDto>> Handle(CreateWarehouseCommand command, CancellationToken ct)
    {
        var result = Warehouse.Create(Guid.NewGuid(), command.Name);
        if (result.IsError) return result.Errors;
        context.Warehouses.Add(result.Value);
        await context.SaveChangesAsync(ct);
        return result.Value.ToDto();
    }
}
