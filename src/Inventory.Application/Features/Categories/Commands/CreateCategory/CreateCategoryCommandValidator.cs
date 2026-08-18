using FluentValidation;
namespace Inventory.Application.Features.Categories.Commands.CreateCategory;
public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator() => RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
}
