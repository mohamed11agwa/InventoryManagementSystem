using Inventory.Application.Common.Interfaces;
using Inventory.Application.Common.Models;
using Inventory.Application.Features.Products.Dtos;
using Inventory.Application.Features.Products.Mappers;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandler(IAppDbContext context)
    : IRequestHandler<GetProductsQuery, Result<PagedResult<ProductDto>>>
{
    public async Task<Result<PagedResult<ProductDto>>> Handle(GetProductsQuery query, CancellationToken ct)
    {
        var products = context.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            products = products.Where(x => x.Name.Contains(search));
        }

        products = query.SortBy.ToLowerInvariant() switch
        {
            "price" => query.Descending ? products.OrderByDescending(x => x.UnitPrice) : products.OrderBy(x => x.UnitPrice),
            _ => query.Descending ? products.OrderByDescending(x => x.Name) : products.OrderBy(x => x.Name)
        };

        var totalCount = await products.CountAsync(ct);
        var page = await products
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(ct);

        return new PagedResult<ProductDto>(page.ToDtos(), query.Page, query.PageSize, totalCount);
    }
}
