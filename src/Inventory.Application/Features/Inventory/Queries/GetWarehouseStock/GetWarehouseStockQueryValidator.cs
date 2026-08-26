using FluentValidation;

namespace Inventory.Application.Features.Inventory.Queries.GetWarehouseStock;

public sealed class GetWarehouseStockQueryValidator : AbstractValidator<GetWarehouseStockQuery>
{
    public GetWarehouseStockQueryValidator()
    {
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.Search).MaximumLength(200).When(x => x.Search is not null);
        RuleFor(x => x.SortBy).Must(x => new[] { "product", "quantity" }.Contains(x, StringComparer.OrdinalIgnoreCase));
    }
}
