using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Orders.Dtos;
using Inventory.Application.Features.Orders.Mappers;
using Inventory.Domain.Common.Results;
using Inventory.Domain.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Features.Orders.Commands.CancelOrder;

public sealed class CancelOrderCommandHandler(
    IAppDbContext context,
    IUser user,
    ILogger<CancelOrderCommandHandler> logger)
    : IRequestHandler<CancelOrderCommand, Result<OrderDto>>
{
    public async Task<Result<OrderDto>> Handle(CancelOrderCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(user.Id))
            return Error.Unauthorized("User.Unauthenticated", "An authenticated user is required.");

        await using var transaction = await context.Database.BeginTransactionAsync(ct);

        var order = await context.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == command.OrderId, ct);

        if (order is null) return Error.NotFound("Order.NotFound", "Order was not found.");

        var wasProcessing = order.Status == OrderStatus.Processing;
        if (order.Status is OrderStatus.Completed or OrderStatus.Cancelled)
            return OrderErrors.InvalidTransition;

        var now = DateTimeOffset.UtcNow;
        var transition = order.Cancel(user.Id, now);
        if (transition.IsError) return transition.Errors;

        if (wasProcessing)
        {
            var productIds = order.Items.Select(x => x.ProductId).Distinct().ToList();
            var warehouseIds = order.Items.Select(x => x.WarehouseId).Distinct().ToList();

            var inventories = await context.Inventories
                .Where(x => productIds.Contains(x.ProductId) && warehouseIds.Contains(x.WarehouseId))
                .ToListAsync(ct);

            foreach (var item in order.Items)
            {
                if (!item.StockConsumed || item.StockRestored)
                    return Error.Conflict("Order.StockRestoreStateInvalid", "The order stock state is invalid for cancellation.");

                var inventory = inventories.FirstOrDefault(x => x.ProductId == item.ProductId && x.WarehouseId == item.WarehouseId);
                if (inventory is null)
                    return Error.NotFound("Inventory.NotFound", "Inventory was not found for an order item.");

                var adjustment = inventory.AdjustStock(
                    item.Quantity,
                    $"Order {order.Id} cancellation",
                    user.Id,
                    now);

                if (adjustment.IsError) return adjustment.Errors;

                var restored = order.MarkItemStockRestored(item.Id);
                if (restored.IsError) return restored.Errors;
            }
        }

        try
        {
            await context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch (DbUpdateConcurrencyException)
        {
            await transaction.RollbackAsync(CancellationToken.None);
            logger.LogWarning("Concurrency conflict while cancelling order {OrderId}", command.OrderId);
            return Error.Conflict("Order.CancellationConcurrencyConflict", "The order or stock was modified by another user. Reload the order and try again.");
        }

        logger.LogInformation("Order {OrderId} was cancelled", order.Id);
        var customerName = await context.Customers.Where(x => x.Id == order.CustomerId).Select(x => x.Name).FirstAsync(ct);
        return order.ToDto(customerName);
    }
}
