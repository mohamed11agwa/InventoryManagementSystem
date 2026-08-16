using Inventory.Domain.Common;

namespace Inventory.Domain.Inventory;

public sealed class StockAdjustment : Entity
{
    public int QuantityChange { get; private set; }
    public int PreviousQuantity { get; private set; }
    public int NewQuantity { get; private set; }
    public DateTimeOffset AdjustedAtUtc { get; private set; }
    public string AdjustedBy { get; private set; } = null!;
    public string Reason { get; private set; } = null!;

    private StockAdjustment()
    { }

    internal StockAdjustment(Guid id, int quantityChange, int previousQuantity, int newQuantity, DateTimeOffset adjustedAtUtc,
        string adjustedBy,
        string reason) : base(id)
    {
        QuantityChange = quantityChange;
        PreviousQuantity = previousQuantity;
        NewQuantity = newQuantity;
        AdjustedAtUtc = adjustedAtUtc;
        AdjustedBy = adjustedBy;
        Reason = reason;
    }
}
