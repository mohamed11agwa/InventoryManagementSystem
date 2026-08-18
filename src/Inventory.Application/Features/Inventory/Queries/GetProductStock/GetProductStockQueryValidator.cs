using FluentValidation;
namespace Inventory.Application.Features.Inventory.Queries.GetProductStock;
public sealed class GetProductStockQueryValidator : AbstractValidator<GetProductStockQuery>
{
    public GetProductStockQueryValidator() => RuleFor(x => x.ProductId).NotEmpty();
}
