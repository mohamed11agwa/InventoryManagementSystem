using Inventory.Application.Features.Inventory.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;
namespace Inventory.Application.Features.Inventory.Commands.AdjustStock;
public sealed record AdjustStockCommand(Guid ProductId, Guid WarehouseId, int QuantityChange, string Reason) : IRequest<Result<InventoryDto>>;
