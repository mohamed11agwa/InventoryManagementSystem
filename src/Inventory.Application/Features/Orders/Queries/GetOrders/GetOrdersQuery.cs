using Inventory.Application.Common.Models;
using Inventory.Application.Features.Orders.Dtos;
using Inventory.Domain.Common.Results;
using Inventory.Domain.Orders;
using MediatR;

namespace Inventory.Application.Features.Orders.Queries.GetOrders;

public sealed record GetOrdersQuery(
    string? Search = null,
    OrderStatus? Status = null,
    int Page = 1,
    int PageSize = 20,
    string SortBy = "createdAt",
    bool Descending = true) : IRequest<Result<PagedResult<OrderDto>>>;
