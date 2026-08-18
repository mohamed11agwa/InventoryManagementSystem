using Inventory.Application.Features.Inventory.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;
namespace Inventory.Application.Features.Inventory.Queries.GetProductStock;
public sealed record GetProductStockQuery(Guid ProductId) : IRequest<Result<List<InventoryDto>>>;
