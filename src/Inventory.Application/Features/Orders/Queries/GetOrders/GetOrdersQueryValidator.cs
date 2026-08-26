using FluentValidation;

namespace Inventory.Application.Features.Orders.Queries.GetOrders;

public sealed class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
{
    public GetOrdersQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.Search).MaximumLength(200).When(x => x.Search is not null);
        RuleFor(x => x.SortBy).Must(x => new[] { "createdAt", "total", "status" }.Contains(x, StringComparer.OrdinalIgnoreCase));
    }
}
