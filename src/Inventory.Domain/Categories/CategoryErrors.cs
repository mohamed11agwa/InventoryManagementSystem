using Inventory.Domain.Common.Results;

namespace Inventory.Domain.Categories;

public static class CategoryErrors
{
    public static Error CategoryIdRequired => Error.Validation(
        code: "CategoryErrors.CategoryIdRequired",
        description: "Category Id is required");

    public static Error NameRequired => Error.Validation(
        code: "CategoryErrors.NameRequired",
        description: "Category name is required");
}
