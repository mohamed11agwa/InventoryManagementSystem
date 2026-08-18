using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Products.Dtos;
using Inventory.Application.Features.Products.Mappers;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler(IAppDbContext context)
    : IRequestHandler<UpdateProductCommand, Result<ProductDto>>
{
    public async Task<Result<ProductDto>> Handle(UpdateProductCommand command, CancellationToken ct)
    {
        var product = await context.Products.FirstOrDefaultAsync(x => x.Id == command.Id, ct);
        if (product is null)
        {
            return Error.NotFound("Product.NotFound", "Product was not found.");
        }

        if (command.CategoryId is not null && !await context.Categories.AnyAsync(x => x.Id == command.CategoryId, ct))
        {
            return Error.NotFound("Category.NotFound", "Category was not found.");
        }

        var result = product.Update(command.Name, command.CategoryId);
        if (result.IsError)
        {
            return result.Errors;
        }

        await context.SaveChangesAsync(ct);
        return product.ToDto();
    }
}
