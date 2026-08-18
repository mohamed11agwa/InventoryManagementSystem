using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Categories.Dtos;
using Inventory.Application.Features.Categories.Mappers;
using Inventory.Domain.Categories;
using Inventory.Domain.Common.Results;
using MediatR;
namespace Inventory.Application.Features.Categories.Commands.CreateCategory;
public sealed class CreateCategoryCommandHandler(IAppDbContext context) : IRequestHandler<CreateCategoryCommand, Result<CategoryDto>>
{
    public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand command, CancellationToken ct)
    {
        var result = Category.Create(Guid.NewGuid(), command.Name);
        if (result.IsError) return result.Errors;
        context.Categories.Add(result.Value);
        await context.SaveChangesAsync(ct);
        return result.Value.ToDto();
    }
}
