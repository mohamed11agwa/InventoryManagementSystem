using Inventory.Domain.Categories;
using Inventory.Domain.Common;
using Inventory.Domain.Common.Results;

namespace Inventory.Domain.Products;

public sealed class Product : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public Guid? CategoryId { get; private set; }
    public Category? Category { get; private set; }

    private Product()
    { }

    private Product(Guid id, string name, Guid? categoryId)
        : base(id)
    {
        Name = name;
        CategoryId = categoryId;
    }

    public static Result<Product> Create(Guid id, string name, Guid? categoryId = null)
    {
        if (id == Guid.Empty)
        {
            return ProductErrors.ProductIdRequired;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return ProductErrors.NameRequired;
        }

        return new Product(id, name.Trim(), categoryId);
    }

    public Result<Updated> Update(string name, Guid? categoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return ProductErrors.NameRequired;
        }

        Name = name.Trim();
        CategoryId = categoryId;

        return Result.Updated;
    }
}
