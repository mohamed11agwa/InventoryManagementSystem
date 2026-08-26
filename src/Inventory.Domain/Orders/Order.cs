using Inventory.Domain.Common;
using Inventory.Domain.Common.Results;

namespace Inventory.Domain.Orders;

public sealed class Order : AuditableEntity
{
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal Total { get; private set; }
    public string IdempotencyKey { get; private set; } = null!;

    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { }

    private Order(Guid id, Guid customerId, string idempotencyKey, IEnumerable<OrderItem> items) : base(id)
    {
        CustomerId = customerId;
        IdempotencyKey = idempotencyKey;
        Status = OrderStatus.Draft;
        _items.AddRange(items);
        Total = _items.Sum(x => x.LineTotal);
    }

    public static Result<Order> Create(Guid id, Guid customerId, string idempotencyKey, IEnumerable<OrderItem> items)
    {
        if (id == Guid.Empty) return OrderErrors.IdRequired;
        if (customerId == Guid.Empty) return OrderErrors.CustomerRequired;
        if (string.IsNullOrWhiteSpace(idempotencyKey)) return OrderErrors.IdempotencyKeyRequired;

        var itemList = items.ToList();
        if (itemList.Count == 0) return OrderErrors.ItemsRequired;
        if (itemList.GroupBy(x => new { x.ProductId, x.WarehouseId }).Any(g => g.Count() > 1))
            return OrderErrors.DuplicateItem;

        return new Order(id, customerId, idempotencyKey.Trim(), itemList);
    }

    public Result<Updated> Confirm(string changedBy, DateTimeOffset changedAtUtc)
        => Transition(OrderStatus.Confirmed, changedBy, changedAtUtc);

    public Result<Updated> StartProcessing(string changedBy, DateTimeOffset changedAtUtc)
        => Transition(OrderStatus.Processing, changedBy, changedAtUtc);

    public Result<Updated> Complete(string changedBy, DateTimeOffset changedAtUtc)
        => Transition(OrderStatus.Completed, changedBy, changedAtUtc);

    public Result<Updated> Cancel(string changedBy, DateTimeOffset changedAtUtc)
        => Status is OrderStatus.Draft or OrderStatus.Confirmed or OrderStatus.Processing
            ? Transition(OrderStatus.Cancelled, changedBy, changedAtUtc)
            : OrderErrors.InvalidTransition;

    public Result<Updated> MarkItemStockConsumed(Guid orderItemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == orderItemId);
        if (item is null) return Error.NotFound("OrderItem.NotFound", "Order item was not found.");
        return item.ConsumeStock();
    }

    public Result<Updated> MarkItemStockRestored(Guid orderItemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == orderItemId);
        if (item is null) return Error.NotFound("OrderItem.NotFound", "Order item was not found.");
        return item.RestoreStock();
    }

    private Result<Updated> Transition(OrderStatus target, string changedBy, DateTimeOffset changedAtUtc)
    {
        if (string.IsNullOrWhiteSpace(changedBy))
            return Error.Unauthorized("Order.ChangedByRequired", "An authenticated user is required.");

        var valid = (Status, target) switch
        {
            (OrderStatus.Draft, OrderStatus.Confirmed) => true,
            (OrderStatus.Confirmed, OrderStatus.Processing) => true,
            (OrderStatus.Processing, OrderStatus.Completed) => true,
            (OrderStatus.Draft, OrderStatus.Cancelled) => true,
            (OrderStatus.Confirmed, OrderStatus.Cancelled) => true,
            (OrderStatus.Processing, OrderStatus.Cancelled) => true,
            _ => false
        };

        if (!valid) return OrderErrors.InvalidTransition;

        var previous = Status;
        Status = target;
        AddDomainEvent(new OrderStatusChangedEvent(Id, previous, target, changedBy, changedAtUtc));
        return Result.Updated;
    }
}
