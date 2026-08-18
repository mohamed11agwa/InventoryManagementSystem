using Inventory.Domain.Common.Results;
using Inventory.Application.Features.Warehouses.Dtos;
using MediatR;
namespace Inventory.Application.Features.Warehouses.Commands.CreateWarehouse;
public sealed record CreateWarehouseCommand(string Name) : IRequest<Result<WarehouseDto>>;
