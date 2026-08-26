using Inventory.Domain.Orders;

namespace Inventory.Application.Features.Orders.Dtos;

public sealed record OrderDto(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    OrderStatus Status,
    decimal Total,
    DateTimeOffset CreatedAtUtc,
    IReadOnlyList<OrderItemDto> Items);

public sealed record OrderItemDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    Guid WarehouseId,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal,
    bool StockConsumed,
    bool StockRestored);

public sealed record OrderStatusHistoryDto(
    Guid Id,
    OrderStatus FromStatus,
    OrderStatus ToStatus,
    DateTimeOffset ChangedAtUtc,
    string ChangedBy);
