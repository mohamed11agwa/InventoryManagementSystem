namespace Inventory.Domain.Orders;

public enum OrderStatus
{
    Draft,
    Confirmed,
    Processing,
    Completed,
    Cancelled
}
