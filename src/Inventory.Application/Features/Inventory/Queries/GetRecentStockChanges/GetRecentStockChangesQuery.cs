using Inventory.Application.Features.Inventory.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;
namespace Inventory.Application.Features.Inventory.Queries.GetRecentStockChanges;
public sealed record GetRecentStockChangesQuery(int Count = 20) : IRequest<Result<List<StockAdjustmentDto>>>;
