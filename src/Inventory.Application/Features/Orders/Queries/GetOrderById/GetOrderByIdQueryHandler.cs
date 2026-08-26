using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Orders.Dtos;
using Inventory.Application.Features.Orders.Mappers;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Orders.Queries.GetOrderById;

public sealed class GetOrderByIdQueryHandler(IAppDbContext context)
    : IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
{
    public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery query, CancellationToken ct)
    {
        var order = await context.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == query.OrderId, ct);

        if (order is null) return Error.NotFound("Order.NotFound", "Order was not found.");

        var customerName = await context.Customers
            .Where(x => x.Id == order.CustomerId)
            .Select(x => x.Name)
            .FirstAsync(ct);

        return order.ToDto(customerName);
    }
}
