using Inventory.Application.Features.Inventory.Dtos;
using Inventory.Domain.Inventory;

namespace Inventory.Application.Features.Inventory.Mappers;

public static class InventoryMapper
{
    public static InventoryDto ToDto(this Inventory.Domain.Inventory.Inventory inventory)
        => new(inventory.Id, inventory.ProductId, inventory.WarehouseId, inventory.Quantity);

    public static StockAdjustmentDto ToDto(this StockAdjustment adjustment, Guid productId, Guid warehouseId)
        => new(adjustment.Id, productId, warehouseId, adjustment.QuantityChange, adjustment.PreviousQuantity,
            adjustment.NewQuantity, adjustment.AdjustedAtUtc, adjustment.AdjustedBy, adjustment.Reason);
}
