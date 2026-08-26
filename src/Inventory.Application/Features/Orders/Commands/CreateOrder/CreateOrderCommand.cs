using Inventory.Application.Features.Orders.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;

namespace Inventory.Application.Features.Orders.Commands.CreateOrder;

public sealed record CreateOrderItemRequest(Guid ProductId, Guid WarehouseId, int Quantity);

public sealed record CreateOrderCommand(
    Guid CustomerId,
    IReadOnlyList<CreateOrderItemRequest> Items,
    string IdempotencyKey) : IRequest<Result<OrderDto>>;
