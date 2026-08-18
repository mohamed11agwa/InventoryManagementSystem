using Inventory.Domain.Common;
using Inventory.Domain.Common.Results;

namespace Inventory.Domain.Inventory;

public sealed class Inventory : AuditableEntity
{
    public Guid ProductId { get; }
    public Guid WarehouseId { get; }
    public int Quantity { get; private set; }

    private readonly List<StockAdjustment> _adjustments = [];
    public IEnumerable<StockAdjustment> Adjustments => _adjustments.AsReadOnly();

    private Inventory()
    { }

    private Inventory(Guid id, Guid productId, Guid warehouseId)
        : base(id)
    {
        ProductId = productId;
        WarehouseId = warehouseId;
        Quantity = 0;
    }

    public static Result<Inventory> Create(Guid id, Guid productId, Guid warehouseId)
    {
        if (id == Guid.Empty)
        {
            return InventoryErrors.InventoryIdRequired;
        }

        if (productId == Guid.Empty)
        {
            return InventoryErrors.ProductIdRequired;
        }

        if (warehouseId == Guid.Empty)
        {
            return InventoryErrors.WarehouseIdRequired;
        }

        return new Inventory(id, productId, warehouseId);
    }

    public Result<Updated> AdjustStock(int quantityChange, string reason, string adjustedBy, DateTimeOffset adjustedAtUtc)
    {
        if (quantityChange == 0)
        {
            return InventoryErrors.AdjustmentCannotBeZero;
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            return InventoryErrors.ReasonRequired;
        }

        if (string.IsNullOrWhiteSpace(adjustedBy))
        {
            return InventoryErrors.AdjustedByRequired;
        }

        var newQuantity = Quantity + quantityChange;

        if (newQuantity < 0)
        {
            return InventoryErrors.InvalidQuantity;
        }

        _adjustments.Add(
            new StockAdjustment(
            Guid.NewGuid(),
            Id,
            quantityChange,
            Quantity,
            newQuantity,
            adjustedAtUtc,
            adjustedBy.Trim(),
            reason.Trim()));

        Quantity = newQuantity;

        return Result.Updated;
    }
}
