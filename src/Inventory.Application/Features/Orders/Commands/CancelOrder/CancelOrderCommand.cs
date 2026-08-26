using Inventory.Application.Features.Orders.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;

namespace Inventory.Application.Features.Orders.Commands.CancelOrder;

public sealed record CancelOrderCommand(Guid OrderId) : IRequest<Result<OrderDto>>;
