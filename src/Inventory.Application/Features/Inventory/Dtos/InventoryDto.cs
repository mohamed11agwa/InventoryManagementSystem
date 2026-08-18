namespace Inventory.Application.Features.Inventory.Dtos;

public sealed record InventoryDto(Guid Id, Guid ProductId, Guid WarehouseId, int Quantity);

public sealed record StockAdjustmentDto(
    Guid Id,
    Guid ProductId,
    Guid WarehouseId,
    int QuantityChange,
    int PreviousQuantity,
    int NewQuantity,
    DateTimeOffset AdjustedAtUtc,
    string AdjustedBy,
    string Reason);
