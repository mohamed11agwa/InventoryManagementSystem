using Inventory.Domain.Common;
using Inventory.Domain.Common.Results;

namespace Inventory.Domain.Warehouses;

public sealed class Warehouse : AuditableEntity
{
    public string Name { get; private set; } = null!;

    private Warehouse()
    { }

    private Warehouse(Guid id, string name)
        : base(id)
    {
        Name = name;
    }

    public static Result<Warehouse> Create(Guid id, string name)
    {
        if (id == Guid.Empty)
        {
            return WarehouseErrors.WarehouseIdRequired;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return WarehouseErrors.NameRequired;
        }

        return new Warehouse(id, name.Trim());
    }

    public Result<Updated> Update(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return WarehouseErrors.NameRequired;
        }

        Name = name.Trim();
        return Result.Updated;
    }
}
