using Inventory.Domain.Common.Results;
using Inventory.Application.Features.Warehouses.Dtos;
using MediatR;
namespace Inventory.Application.Features.Warehouses.Commands.UpdateWarehouse;
public sealed record UpdateWarehouseCommand(Guid Id, string Name) : IRequest<Result<WarehouseDto>>;
