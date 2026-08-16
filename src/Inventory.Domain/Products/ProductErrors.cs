using Inventory.Domain.Common.Results;

namespace Inventory.Domain.Products;

public static class ProductErrors
{
    public static Error ProductIdRequired => Error.Validation(
        code: "ProductErrors.ProductIdRequired",
        description: "Product Id is required");

    public static Error NameRequired => Error.Validation(
        code: "ProductErrors.NameRequired",
        description: "Product name is required");
}
