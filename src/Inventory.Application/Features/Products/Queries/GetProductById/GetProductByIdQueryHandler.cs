using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Products.Dtos;
using Inventory.Application.Features.Products.Mappers;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler(IAppDbContext context)
    : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery query, CancellationToken ct)
    {
        var product = await context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == query.Id, ct);
        return product is null
            ? Error.NotFound("Product.NotFound", "Product was not found.")
            : product.ToDto();
    }
}
