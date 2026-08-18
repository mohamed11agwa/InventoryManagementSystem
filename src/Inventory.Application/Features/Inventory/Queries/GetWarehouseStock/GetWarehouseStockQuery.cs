using Inventory.Application.Features.Inventory.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;
namespace Inventory.Application.Features.Inventory.Queries.GetWarehouseStock;
public sealed record GetWarehouseStockQuery(Guid WarehouseId) : IRequest<Result<List<InventoryDto>>>;
