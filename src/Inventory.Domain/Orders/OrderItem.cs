using Inventory.Domain.Common;
using Inventory.Domain.Common.Results;

namespace Inventory.Domain.Orders;

public sealed class OrderItem : Entity
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid WarehouseId { get; private set; }
    public string ProductName { get; private set; } = null!;
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal LineTotal => UnitPrice * Quantity;
    public bool StockConsumed { get; private set; }
    public bool StockRestored { get; private set; }

    private OrderItem() { }

    private OrderItem(Guid id, Guid productId, Guid warehouseId, string productName, decimal unitPrice, int quantity) : base(id)
    {
        ProductId = productId;
        WarehouseId = warehouseId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public static Result<OrderItem> Create(Guid id, Guid productId, Guid warehouseId, string productName, decimal unitPrice, int quantity)
    {
        if (id == Guid.Empty)
            return Error.Validation("OrderItem.IdRequired", "Order item id is required.");
        if (productId == Guid.Empty)
            return Error.Validation("OrderItem.ProductRequired", "Product is required.");
        if (warehouseId == Guid.Empty)
            return Error.Validation("OrderItem.WarehouseRequired", "Warehouse is required.");
        if (string.IsNullOrWhiteSpace(productName))
            return Error.Validation("OrderItem.ProductNameRequired", "Product name is required.");
        if (unitPrice < 0)
            return Error.Validation("OrderItem.InvalidUnitPrice", "Unit price cannot be negative.");
        if (quantity <= 0)
            return Error.Validation("OrderItem.InvalidQuantity", "Quantity must be greater than zero.");

        return new OrderItem(id, productId, warehouseId, productName.Trim(), unitPrice, quantity);
    }

    internal Result<Updated> ConsumeStock()
    {
        if (StockConsumed) return OrderErrors.AlreadyProcessed;
        StockConsumed = true;
        return Result.Updated;
    }

    internal Result<Updated> RestoreStock()
    {
        if (!StockConsumed) return OrderErrors.StockNotConsumed;
        if (StockRestored) return OrderErrors.StockAlreadyRestored;
        StockRestored = true;
        return Result.Updated;
    }
}
