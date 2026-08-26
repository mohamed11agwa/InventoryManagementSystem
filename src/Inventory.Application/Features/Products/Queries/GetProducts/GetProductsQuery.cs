using Inventory.Application.Common.Models;
using Inventory.Application.Features.Products.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;

namespace Inventory.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductsQuery(
    string? Search = null,
    int Page = 1,
    int PageSize = 20,
    string SortBy = "name",
    bool Descending = false) : IRequest<Result<PagedResult<ProductDto>>>;
