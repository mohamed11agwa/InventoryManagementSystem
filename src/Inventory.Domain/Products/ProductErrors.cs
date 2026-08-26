using Inventory.Domain.Common.Results;

namespace Inventory.Domain.Products;

public static class ProductErrors
{
    public static Error ProductIdRequired =>
        Error.Validation("ProductErrors.ProductIdRequired", "Product Id is required");
    public static Error NameRequired =>
        Error.Validation("ProductErrors.NameRequired", "Product name is required");
    public static Error InvalidUnitPrice =>
        Error.Validation("ProductErrors.InvalidUnitPrice", "Product unit price cannot be negative.");
}