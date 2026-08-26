using Inventory.Application.Features.Orders.Dtos;
using Inventory.Domain.Orders;

namespace Inventory.Application.Features.Orders.Mappers;

public static class OrderMapper
{
    public static OrderDto ToDto(this Order order, string customerName)
        => new(
            order.Id,
            order.CustomerId,
            customerName,
            order.Status,
            order.Total,
            order.CreatedAtUtc,
            order.Items.Select(x => new OrderItemDto(
                x.Id,
                x.ProductId,
                x.ProductName,
                x.WarehouseId,
                x.UnitPrice,
                x.Quantity,
                x.LineTotal,
                x.StockConsumed,
                x.StockRestored)).ToList());

    public static OrderStatusHistoryDto ToDto(this OrderStatusHistory history, string changedByName)
        => new(history.Id, history.FromStatus, history.ToStatus, history.ChangedAtUtc, changedByName);
}
