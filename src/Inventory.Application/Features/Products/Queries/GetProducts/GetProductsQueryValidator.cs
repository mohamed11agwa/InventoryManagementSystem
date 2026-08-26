using FluentValidation;

namespace Inventory.Application.Features.Products.Queries.GetProducts;

public sealed class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.Search).MaximumLength(200).When(x => x.Search is not null);
        RuleFor(x => x.SortBy).Must(x => new[] { "name", "price" }.Contains(x, StringComparer.OrdinalIgnoreCase));
    }
}
