using Inventory.Application.Features.Orders.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;

namespace Inventory.Application.Features.Orders.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(Guid OrderId) : IRequest<Result<OrderDto>>;
