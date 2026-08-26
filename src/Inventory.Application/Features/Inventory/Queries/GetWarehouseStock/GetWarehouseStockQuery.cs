using Inventory.Application.Common.Models;
using Inventory.Application.Features.Inventory.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;

namespace Inventory.Application.Features.Inventory.Queries.GetWarehouseStock;

public sealed record GetWarehouseStockQuery(
    Guid WarehouseId,
    string? Search = null,
    int Page = 1,
    int PageSize = 20,
    string SortBy = "product",
    bool Descending = false) : IRequest<Result<PagedResult<InventoryDto>>>;
