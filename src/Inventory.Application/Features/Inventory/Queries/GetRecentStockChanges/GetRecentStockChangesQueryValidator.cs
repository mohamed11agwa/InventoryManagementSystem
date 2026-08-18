using FluentValidation;
namespace Inventory.Application.Features.Inventory.Queries.GetRecentStockChanges;
public sealed class GetRecentStockChangesQueryValidator : AbstractValidator<GetRecentStockChangesQuery>
{
    public GetRecentStockChangesQueryValidator()
    {
        RuleFor(x => x.Count).InclusiveBetween(1, 100);
    }
}
