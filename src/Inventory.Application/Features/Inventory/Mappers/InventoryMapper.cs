//using Inventory.Application.Features.Inventory.Dtos;
//using Inventory.Domain.Inventory;
//using InventoryEntity = Inventory.Domain.Inventory.Inventory;

//namespace Inventory.Application.Features.Inventory.Mappers;

//public static class InventoryMapper
//{
//    public static InventoryDto ToDto(this InventoryEntity inventory, string productName, string warehouseName)
//        => new(inventory.Id,
//               productName,
//               warehouseName,
//               inventory.Quantity
//            );

//    public static StockAdjustmentDto ToDto(this StockAdjustment adjustment, Guid productId,string productName,Guid warehouseId,
//        string warehouseName,
//        string adjustedBy) 
//            => new(
//            adjustment.Id,
//            productId,
//            productName,
//            warehouseId,
//            warehouseName,
//            adjustment.QuantityChange,
//            adjustment.PreviousQuantity,
//            adjustment.NewQuantity,
//            adjustment.AdjustedAtUtc,
//            adjustedBy,
//            adjustment.Reason);
//}
using Inventory.Application.Features.Inventory.Dtos;
using InventoryEntity = Inventory.Domain.Inventory.Inventory;
using Inventory.Domain.Inventory;

namespace Inventory.Application.Features.Inventory.Mappers;

public static class InventoryMapper
{
    public static InventoryDto ToDto(
        this InventoryEntity inventory,
        string productName,
        string warehouseName)
        => new(
            inventory.Id,
            productName,
            warehouseName,
            inventory.Quantity);

    public static StockAdjustmentDto ToDto(
    this StockAdjustment adjustment,
    string productName,
    string warehouseName,
    string adjustedByName)
    => new(
        adjustment.Id,
        productName,
        warehouseName,
        adjustment.QuantityChange,
        adjustment.PreviousQuantity,
        adjustment.NewQuantity,
        adjustment.AdjustedAtUtc,
        adjustedByName,
        adjustment.Reason);
}