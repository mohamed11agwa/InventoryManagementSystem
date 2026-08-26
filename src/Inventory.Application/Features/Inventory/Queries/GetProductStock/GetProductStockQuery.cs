using Inventory.Application.Common.Models;
using Inventory.Application.Features.Inventory.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;

namespace Inventory.Application.Features.Inventory.Queries.GetProductStock;

public sealed record GetProductStockQuery(
    Guid ProductId,
    string? Search = null,
    int Page = 1,
    int PageSize = 20,
    string SortBy = "warehouse",
    bool Descending = false) : IRequest<Result<PagedResult<InventoryDto>>>;
