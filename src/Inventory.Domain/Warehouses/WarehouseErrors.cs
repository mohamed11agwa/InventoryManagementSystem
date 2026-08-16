using Inventory.Domain.Common.Results;

namespace Inventory.Domain.Warehouses;

public static class WarehouseErrors
{
    public static Error WarehouseIdRequired => Error.Validation(
        code: "WarehouseErrors.WarehouseIdRequired",
        description: "Warehouse Id is required");

    public static Error NameRequired => Error.Validation(
        code: "WarehouseErrors.NameRequired",
        description: "Warehouse name is required");
}
