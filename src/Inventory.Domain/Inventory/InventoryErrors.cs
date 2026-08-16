using Inventory.Domain.Common.Results;

namespace Inventory.Domain.Inventory;

public static class InventoryErrors
{
    public static Error InventoryIdRequired => Error.Validation(
        code: "InventoryErrors.InventoryIdRequired",
        description: "Inventory Id is required");

    public static Error ProductIdRequired => Error.Validation(
        code: "InventoryErrors.ProductIdRequired",
        description: "Product Id is required");

    public static Error WarehouseIdRequired => Error.Validation(
        code: "InventoryErrors.WarehouseIdRequired",
        description: "Warehouse Id is required");

    public static Error AdjustmentCannotBeZero => Error.Validation(
        code: "InventoryErrors.AdjustmentCannotBeZero",
        description: "Stock adjustment cannot be zero.");

    public static Error ReasonRequired => Error.Validation(
        code: "InventoryErrors.ReasonRequired",
        description: "Stock adjustment reason is required.");

    public static Error AdjustedByRequired => Error.Validation(
        code: "InventoryErrors.AdjustedByRequired",
        description: "Stock adjustment actor is required.");

    public static Error InvalidQuantity => Error.Conflict(
        code: "InventoryErrors.InvalidQuantity",
        description: "Stock quantity cannot be negative.");
}
