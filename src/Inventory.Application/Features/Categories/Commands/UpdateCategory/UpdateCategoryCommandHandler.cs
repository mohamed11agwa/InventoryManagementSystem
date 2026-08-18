using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Categories.Dtos;
using Inventory.Application.Features.Categories.Mappers;
using Inventory.Domain.Categories;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Inventory.Application.Features.Categories.Commands.UpdateCategory;
public sealed class UpdateCategoryCommandHandler(IAppDbContext context) : IRequestHandler<UpdateCategoryCommand, Result<CategoryDto>>
{
    public async Task<Result<CategoryDto>> Handle(UpdateCategoryCommand command, CancellationToken ct)
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == command.Id, ct);
        if (category is null) return Error.NotFound("Category.NotFound", "Category was not found.");
        var result = category.Update(command.Name);
        if (result.IsError) return result.Errors;
        await context.SaveChangesAsync(ct);
        return category.ToDto();
    }
}
