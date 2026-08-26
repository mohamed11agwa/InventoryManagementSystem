using FluentValidation;

namespace Inventory.Application.Features.Inventory.Queries.GetProductStock;

public sealed class GetProductStockQueryValidator : AbstractValidator<GetProductStockQuery>
{
    public GetProductStockQueryValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.Search).MaximumLength(200).When(x => x.Search is not null);
        RuleFor(x => x.SortBy).Must(x => new[] { "warehouse", "quantity" }.Contains(x, StringComparer.OrdinalIgnoreCase));
    }
}
