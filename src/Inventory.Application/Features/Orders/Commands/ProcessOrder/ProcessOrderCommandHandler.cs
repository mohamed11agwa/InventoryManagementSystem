using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Orders.Dtos;
using Inventory.Application.Features.Orders.Mappers;
using Inventory.Domain.Common.Results;
using Inventory.Domain.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Features.Orders.Commands.ProcessOrder;

public sealed class ProcessOrderCommandHandler(
    IAppDbContext context,
    IUser user,
    ILogger<ProcessOrderCommandHandler> logger)
    : IRequestHandler<ProcessOrderCommand, Result<OrderDto>>
{
    public async Task<Result<OrderDto>> Handle(ProcessOrderCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(user.Id))
            return Error.Unauthorized("User.Unauthenticated", "An authenticated user is required.");

        await using var transaction = await context.Database.BeginTransactionAsync(ct);

        var order = await context.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == command.OrderId, ct);

        if (order is null) return Error.NotFound("Order.NotFound", "Order was not found.");
        if (order.Status != OrderStatus.Confirmed) return OrderErrors.InvalidTransition;

        var inventoryKeys = order.Items.Select(x => new { x.ProductId, x.WarehouseId }).ToList();
        var productIds = inventoryKeys.Select(x => x.ProductId).Distinct().ToList();
        var warehouseIds = inventoryKeys.Select(x => x.WarehouseId).Distinct().ToList();

        var inventories = await context.Inventories
            .Where(x => productIds.Contains(x.ProductId) && warehouseIds.Contains(x.WarehouseId))
            .ToListAsync(ct);

        foreach (var item in order.Items)
        {
            var inventory = inventories.FirstOrDefault(x => x.ProductId == item.ProductId && x.WarehouseId == item.WarehouseId);
            if (inventory is null)
                return Error.NotFound("Inventory.NotFound", $"Inventory was not found for product {item.ProductId} in warehouse {item.WarehouseId}.");

            if (inventory.Quantity < item.Quantity)
                return Error.Conflict("Inventory.InsufficientStock", "There is not enough stock to process the order.");
        }

        var now = DateTimeOffset.UtcNow;
        var transition = order.StartProcessing(user.Id, now);
        if (transition.IsError) return transition.Errors;

        foreach (var item in order.Items)
        {
            var inventory = inventories.First(x => x.ProductId == item.ProductId && x.WarehouseId == item.WarehouseId);
            var adjustment = inventory.AdjustStock(
                -item.Quantity,
                $"Order {order.Id} processing",
                user.Id,
                now);

            if (adjustment.IsError) return adjustment.Errors;

            var consumed = order.MarkItemStockConsumed(item.Id);
            if (consumed.IsError) return consumed.Errors;
        }

        try
        {
            await context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch (DbUpdateConcurrencyException)
        {
            await transaction.RollbackAsync(CancellationToken.None);
            logger.LogWarning("Concurrency conflict while processing order {OrderId}", command.OrderId);
            return Error.Conflict("Order.ProcessingConcurrencyConflict", "The order or stock was modified by another user. Reload the order and try again.");
        }

        logger.LogInformation("Order {OrderId} moved to Processing and stock was consumed", order.Id);
        var customerName = await context.Customers.Where(x => x.Id == order.CustomerId).Select(x => x.Name).FirstAsync(ct);
        return order.ToDto(customerName);
    }
}
