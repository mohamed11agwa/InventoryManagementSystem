using Inventory.Application.Features.Orders.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;

namespace Inventory.Application.Features.Orders.Queries.GetOrderHistory;

public sealed record GetOrderHistoryQuery(Guid OrderId) : IRequest<Result<List<OrderStatusHistoryDto>>>;
