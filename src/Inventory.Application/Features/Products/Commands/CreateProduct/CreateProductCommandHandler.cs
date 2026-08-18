using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Products.Dtos;
using Inventory.Application.Features.Products.Mappers;
using Inventory.Domain.Common.Results;
using Inventory.Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler(IAppDbContext context)
    : IRequestHandler<CreateProductCommand, Result<ProductDto>>
{
    public async Task<Result<ProductDto>> Handle(CreateProductCommand command, CancellationToken ct)
    {
        if (command.CategoryId is not null && !await context.Categories.AnyAsync(x => x.Id == command.CategoryId, ct))
        {
            return Error.NotFound("Category.NotFound", "Category was not found.");
        }

        var result = Product.Create(Guid.NewGuid(), command.Name, command.CategoryId);

        if (result.IsError)
        {
            return result.Errors;
        }

        context.Products.Add(result.Value);
        await context.SaveChangesAsync(ct);

        return result.Value.ToDto();
    }
}
