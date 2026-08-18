using Inventory.Application.Features.Inventory.Dtos;
using InventoryEntity = Inventory.Domain.Inventory.Inventory;
using Inventory.Domain.Inventory;

namespace Inventory.Application.Features.Inventory.Mappers;

public static class InventoryMapper
{
    public static InventoryDto ToDto(this InventoryEntity inventory) => new(
            inventory.Id,
            inventory.ProductId,
            inventory.WarehouseId,
            inventory.Quantity
    );

    public static StockAdjustmentDto ToDto(
    this StockAdjustment adjustment,
    string productName,
    string warehouseName,
    string adjustedBy) => new(
        adjustment.Id,
        productName,
        warehouseName,
        adjustment.QuantityChange,
        adjustment.PreviousQuantity,
        adjustment.NewQuantity,
        adjustment.AdjustedAtUtc,
        adjustedBy,
        adjustment.Reason);
}