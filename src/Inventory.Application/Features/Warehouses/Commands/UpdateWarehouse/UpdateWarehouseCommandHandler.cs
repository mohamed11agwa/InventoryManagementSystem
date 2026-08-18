using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Warehouses.Dtos;
using Inventory.Application.Features.Warehouses.Mappers;
using Inventory.Domain.Common.Results;
using Inventory.Domain.Warehouses;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Inventory.Application.Features.Warehouses.Commands.UpdateWarehouse;
public sealed class UpdateWarehouseCommandHandler(IAppDbContext context) : IRequestHandler<UpdateWarehouseCommand, Result<WarehouseDto>>
{
    public async Task<Result<WarehouseDto>> Handle(UpdateWarehouseCommand command, CancellationToken ct)
    {
        var warehouse = await context.Warehouses.FirstOrDefaultAsync(x => x.Id == command.Id, ct);
        if (warehouse is null) return Error.NotFound("Warehouse.NotFound", "Warehouse was not found.");
        var result = warehouse.Update(command.Name);
        if (result.IsError) return result.Errors;
        await context.SaveChangesAsync(ct);
        return warehouse.ToDto();
    }
}
