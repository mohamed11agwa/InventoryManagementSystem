using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Orders.Dtos;
using Inventory.Application.Features.Orders.Mappers;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Orders.Commands.CompleteOrder;

public sealed class CompleteOrderCommandHandler(IAppDbContext context, IUser user)
    : IRequestHandler<CompleteOrderCommand, Result<OrderDto>>
{
    public async Task<Result<OrderDto>> Handle(CompleteOrderCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(user.Id))
            return Error.Unauthorized("User.Unauthenticated", "An authenticated user is required.");

        var order = await context.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == command.OrderId, ct);

        if (order is null) return Error.NotFound("Order.NotFound", "Order was not found.");

        var result = order.Complete(user.Id, DateTimeOffset.UtcNow);
        if (result.IsError) return result.Errors;

        try
        {
            await context.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict("Order.ConcurrencyConflict", "The order was modified by another user. Please reload it and try again.");
        }

        var customerName = await context.Customers.Where(x => x.Id == order.CustomerId).Select(x => x.Name).FirstAsync(ct);
        return order.ToDto(customerName);
    }
}
