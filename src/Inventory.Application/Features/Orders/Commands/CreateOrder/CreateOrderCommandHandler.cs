using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Orders.Dtos;
using Inventory.Application.Features.Orders.Mappers;
using Inventory.Domain.Common.Results;
using Inventory.Domain.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Features.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandHandler(
    IAppDbContext context,
    ILogger<CreateOrderCommandHandler> logger)
    : IRequestHandler<CreateOrderCommand, Result<OrderDto>>
{
    public async Task<Result<OrderDto>> Handle(CreateOrderCommand command, CancellationToken ct)
    {
        var existing = await context.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.IdempotencyKey == command.IdempotencyKey.Trim(), ct);

        if (existing is not null)
        {
            var customerName = await context.Customers
                .Where(x => x.Id == existing.CustomerId)
                .Select(x => x.Name)
                .FirstAsync(ct);

            logger.LogInformation("Returning existing order {OrderId} for idempotency key {IdempotencyKey}", existing.Id, command.IdempotencyKey);
            return existing.ToDto(customerName);
        }

        if (!await context.Customers.AnyAsync(x => x.Id == command.CustomerId, ct))
            return Error.NotFound("Customer.NotFound", "Customer was not found.");

        var productIds = command.Items.Select(x => x.ProductId).Distinct().ToList();
        var warehouseIds = command.Items.Select(x => x.WarehouseId).Distinct().ToList();

        var products = await context.Products
            .Where(x => productIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, ct);

        if (products.Count != productIds.Count)
            return Error.NotFound("Product.NotFound", "One or more products were not found.");

        var warehouses = await context.Warehouses
            .Where(x => warehouseIds.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync(ct);

        if (warehouses.Count != warehouseIds.Count)
            return Error.NotFound("Warehouse.NotFound", "One or more warehouses were not found.");

        var items = new List<OrderItem>();
        foreach (var request in command.Items)
        {
            var product = products[request.ProductId];
            var itemResult = OrderItem.Create(
                Guid.NewGuid(),
                request.ProductId,
                request.WarehouseId,
                product.Name,
                product.UnitPrice,
                request.Quantity);

            if (itemResult.IsError)
                return itemResult.Errors;
            items.Add(itemResult.Value);
        }

        var orderResult = Order.Create(Guid.NewGuid(), command.CustomerId, command.IdempotencyKey, items);
        if (orderResult.IsError) return orderResult.Errors;

        context.Orders.Add(orderResult.Value);

        try
        {
            await context.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            var existingOrder = await context.Orders
                .AsNoTracking()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.IdempotencyKey == command.IdempotencyKey.Trim(), ct);

            if (existingOrder is null) throw;

            var customerName = await context.Customers
                .Where(x => x.Id == existingOrder.CustomerId)
                .Select(x => x.Name)
                .FirstAsync(ct);

            logger.LogInformation("Idempotency race resolved to existing order {OrderId} for key {IdempotencyKey}", existingOrder.Id, command.IdempotencyKey);
            return existingOrder.ToDto(customerName);
        }

        var createdCustomerName = await context.Customers
            .Where(x => x.Id == orderResult.Value.CustomerId)
            .Select(x => x.Name)
            .FirstAsync(ct);

        logger.LogInformation("Created order {OrderId} for customer {CustomerId}", orderResult.Value.Id, orderResult.Value.CustomerId);
        return orderResult.Value.ToDto(createdCustomerName);
    }

}
