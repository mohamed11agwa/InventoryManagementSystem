using Inventory.Domain.Common;

namespace Inventory.Domain.Orders;

public sealed class OrderStatusHistory : Entity
{
    public Guid OrderId { get; private set; }
    public OrderStatus FromStatus { get; private set; }
    public OrderStatus ToStatus { get; private set; }
    public DateTimeOffset ChangedAtUtc { get; private set; }
    public string ChangedBy { get; private set; } = null!;

    private OrderStatusHistory() { }

    public OrderStatusHistory(Guid id, Guid orderId, OrderStatus fromStatus, OrderStatus toStatus, DateTimeOffset changedAtUtc, string changedBy)
        : base(id)
    {
        OrderId = orderId;
        FromStatus = fromStatus;
        ToStatus = toStatus;
        ChangedAtUtc = changedAtUtc;
        ChangedBy = changedBy;
    }
}
