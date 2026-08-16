using Inventory.Domain.Common;
using Inventory.Domain.Common.Results;

namespace Inventory.Domain.Categories;

public sealed class Category : AuditableEntity
{
    public string Name { get; private set; } = default!;

    private Category()
    { }

    private Category(Guid id, string name)
        : base(id)
    {
        Name = name;
    }

    public static Result<Category> Create(Guid id, string name)
    {
        if (id == Guid.Empty)
        {
            return CategoryErrors.CategoryIdRequired;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return CategoryErrors.NameRequired;
        }

        return new Category(id, name.Trim());
    }

    public Result<Updated> Update(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return CategoryErrors.NameRequired;
        }

        Name = name.Trim();
        return Result.Updated;
    }
}
