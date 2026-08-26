using Inventory.Domain.Common.Results;

namespace Inventory.Domain.Orders;

public static class OrderErrors
{
    public static Error IdRequired =>
        Error.Validation("Order.IdRequired", "Order id is required.");
    public static Error CustomerRequired =>
        Error.Validation("Order.CustomerRequired", "Customer is required.");
    public static Error IdempotencyKeyRequired =>
        Error.Validation("Order.IdempotencyKeyRequired", "Idempotency key is required.");
    public static Error ItemsRequired =>
        Error.Validation("Order.ItemsRequired", "At least one order item is required.");
    public static Error DuplicateItem =>
        Error.Validation("Order.DuplicateItem", "The same product cannot be added more than once for the same warehouse.");
    public static Error InvalidTransition =>
        Error.Conflict("Order.InvalidTransition", "The requested order state transition is not valid.");
    public static Error AlreadyProcessed =>
        Error.Conflict("Order.AlreadyProcessed", "The order has already been processed.");
    public static Error StockNotConsumed =>
        Error.Conflict("Order.StockNotConsumed", "Order stock has not been consumed.");
    public static Error StockAlreadyRestored =>
        Error.Conflict("Order.StockAlreadyRestored", "Order stock has already been restored.");
}
