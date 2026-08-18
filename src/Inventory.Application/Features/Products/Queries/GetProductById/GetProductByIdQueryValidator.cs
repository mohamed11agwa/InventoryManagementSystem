using FluentValidation;

namespace Inventory.Application.Features.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator() => RuleFor(x => x.Id).NotEmpty();
}
