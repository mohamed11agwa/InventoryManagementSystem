using Inventory.Application.Features.Orders.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;

namespace Inventory.Application.Features.Orders.Commands.CompleteOrder;

public sealed record CompleteOrderCommand(Guid OrderId) : IRequest<Result<OrderDto>>;
