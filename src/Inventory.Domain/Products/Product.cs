using Inventory.Domain.Categories;
using Inventory.Domain.Common;
using Inventory.Domain.Common.Results;

namespace Inventory.Domain.Products;

public sealed class Product : AuditableEntity
{

    public string Name { get; private set; } = null!;
    public decimal UnitPrice { get; private set; }
    public Guid? CategoryId { get; private set; }
    public Category? Category { get; private set; }

    private Product() { }

    private Product(Guid id, string name, decimal unitPrice, Guid? categoryId) : base(id)
    {
        Name = name;
        UnitPrice = unitPrice;
        CategoryId = categoryId;
    }

    public static Result<Product> Create(Guid id, string name, decimal unitPrice, Guid? categoryId = null)
    {
        if (id == Guid.Empty)
            return ProductErrors.ProductIdRequired;
        if (string.IsNullOrWhiteSpace(name))
            return ProductErrors.NameRequired;
        if (unitPrice < 0)
            return ProductErrors.InvalidUnitPrice;

        return new Product(id, name.Trim(), unitPrice, categoryId);
    }

    public Result<Updated> Update(string name, decimal unitPrice, Guid? categoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
            return ProductErrors.NameRequired;
        if (unitPrice < 0)
            return ProductErrors.InvalidUnitPrice;

        Name = name.Trim();
        UnitPrice = unitPrice;
        CategoryId = categoryId;
        return Result.Updated;
    }
}
