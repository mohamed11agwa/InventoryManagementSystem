using Inventory.Domain.Common;

namespace Inventory.Domain.Orders;

public sealed class OrderStatusChangedEvent : DomainEvent
{
    public Guid OrderId { get; }
    public OrderStatus From { get; }
    public OrderStatus To { get; }
    public string ChangedBy { get; }
    public DateTimeOffset ChangedAtUtc { get; }

    public OrderStatusChangedEvent(
        Guid orderId,
        OrderStatus from,
        OrderStatus to,
        string changedBy,
        DateTimeOffset changedAtUtc)
    {
        OrderId = orderId;
        From = from;
        To = to;
        ChangedBy = changedBy;
        ChangedAtUtc = changedAtUtc;
    }
}
