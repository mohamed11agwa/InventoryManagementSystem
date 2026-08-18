using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Products.Dtos;
using Inventory.Application.Features.Products.Mappers;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandler(IAppDbContext context)
    : IRequestHandler<GetProductsQuery, Result<List<ProductDto>>>
{
    public async Task<Result<List<ProductDto>>> Handle(GetProductsQuery query, CancellationToken ct)
    {
        var products = await context.Products.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct);
        return products.ToDtos();
    }
}
