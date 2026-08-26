using Inventory.Application.Features.Orders.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;

namespace Inventory.Application.Features.Orders.Commands.ConfirmOrder;

public sealed record ConfirmOrderCommand(Guid OrderId) : IRequest<Result<OrderDto>>;
