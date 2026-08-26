using Inventory.Domain.Common;
using Inventory.Domain.Common.Results;

namespace Inventory.Domain.Customers;

public sealed class Customer : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;

    private Customer() { }

    private Customer(Guid id, string name, string email) : base(id)
    {
        Name = name;
        Email = email;
    }

    public static Result<Customer> Create(Guid id, string name, string email)
    {
        if (id == Guid.Empty)
            return CustomerErrors.IdRequired;
        if (string.IsNullOrWhiteSpace(name))
            return CustomerErrors.NameRequired;
        if (string.IsNullOrWhiteSpace(email))
            return CustomerErrors.EmailRequired;

        return new Customer(id, name.Trim(), email.Trim());
    }

    public Result<Updated> Update(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            return CustomerErrors.NameRequired;
        if (string.IsNullOrWhiteSpace(email))
            return CustomerErrors.EmailRequired;

        Name = name.Trim();
        Email = email.Trim();
        return Result.Updated;
    }
}
