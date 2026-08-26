using Inventory.Application.Common.Interfaces;
using Inventory.Application.Common.Models;
using Inventory.Application.Features.Orders.Dtos;
using Inventory.Application.Features.Orders.Mappers;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Orders.Queries.GetOrders;

public sealed class GetOrdersQueryHandler(IAppDbContext context)
    : IRequestHandler<GetOrdersQuery, Result<PagedResult<OrderDto>>>
{
    public async Task<Result<PagedResult<OrderDto>>> Handle(GetOrdersQuery query, CancellationToken ct)
    {
        var baseQuery = context.Orders.AsNoTracking();

        if (query.Status is not null)
            baseQuery = baseQuery.Where(x => x.Status == query.Status);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            baseQuery = baseQuery.Where(x =>
                x.Id.ToString().Contains(search) ||
                context.Customers.Any(c => c.Id == x.CustomerId && c.Name.Contains(search)));
        }

        baseQuery = query.SortBy.ToLowerInvariant() switch
        {
            "total" => query.Descending ? baseQuery.OrderByDescending(x => x.Total) : baseQuery.OrderBy(x => x.Total),
            "status" => query.Descending ? baseQuery.OrderByDescending(x => x.Status) : baseQuery.OrderBy(x => x.Status),
            _ => query.Descending ? baseQuery.OrderByDescending(x => x.CreatedAtUtc) : baseQuery.OrderBy(x => x.CreatedAtUtc)
        };

        var totalCount = await baseQuery.CountAsync(ct);

        var orders = await baseQuery
            .Include(x => x.Items)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(ct);

        var customerIds = orders.Select(x => x.CustomerId).Distinct().ToList();
        var customerNames = await context.Customers
            .Where(x => customerIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x.Name, ct);

        var items = orders
            .Select(x => x.ToDto(customerNames[x.CustomerId]))
            .ToList();

        return new PagedResult<OrderDto>(items, query.Page, query.PageSize, totalCount);
    }
}
