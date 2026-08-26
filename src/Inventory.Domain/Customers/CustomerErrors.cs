using Inventory.Domain.Common.Results;

namespace Inventory.Domain.Customers;

public static class CustomerErrors
{
    public static Error IdRequired =>
        Error.Validation("Customer.IdRequired", "Customer id is required.");
    public static Error NameRequired =>
        Error.Validation("Customer.NameRequired", "Customer name is required.");
    public static Error EmailRequired =>
        Error.Validation("Customer.EmailRequired", "Customer email is required.");
}
