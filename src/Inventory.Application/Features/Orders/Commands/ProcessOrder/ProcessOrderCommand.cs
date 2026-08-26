using Inventory.Application.Features.Orders.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;

namespace Inventory.Application.Features.Orders.Commands.ProcessOrder;

public sealed record ProcessOrderCommand(Guid OrderId) : IRequest<Result<OrderDto>>;
