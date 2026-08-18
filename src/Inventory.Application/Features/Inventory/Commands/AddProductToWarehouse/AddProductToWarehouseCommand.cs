using Inventory.Application.Features.Inventory.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;
namespace Inventory.Application.Features.Inventory.Commands.AddProductToWarehouse;
public sealed record AddProductToWarehouseCommand(Guid ProductId, Guid WarehouseId) : IRequest<Result<InventoryDto>>;
