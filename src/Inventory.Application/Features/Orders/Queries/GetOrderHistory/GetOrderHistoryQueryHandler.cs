using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Orders.Dtos;
using Inventory.Application.Features.Orders.Mappers;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Orders.Queries.GetOrderHistory;

public sealed class GetOrderHistoryQueryHandler(IAppDbContext context, IUserLookup userLookup)
    : IRequestHandler<GetOrderHistoryQuery, Result<List<OrderStatusHistoryDto>>>
{
    public async Task<Result<List<OrderStatusHistoryDto>>> Handle(GetOrderHistoryQuery query, CancellationToken ct)
    {
        if (!await context.Orders.AnyAsync(x => x.Id == query.OrderId, ct))
            return Error.NotFound("Order.NotFound", "Order was not found.");

        var history = await context.OrderStatusHistories
            .AsNoTracking()
            .Where(x => x.OrderId == query.OrderId)
            .OrderBy(x => x.ChangedAtUtc)
            .ToListAsync(ct);

        var userNames = await userLookup.GetUserNamesAsync(history.Select(x => x.ChangedBy), ct);
        return history.Select(x => x.ToDto(userNames.GetValueOrDefault(x.ChangedBy, x.ChangedBy))).ToList();
    }
}
