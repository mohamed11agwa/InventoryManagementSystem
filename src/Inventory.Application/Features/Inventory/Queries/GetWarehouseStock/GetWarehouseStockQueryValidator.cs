using FluentValidation;
namespace Inventory.Application.Features.Inventory.Queries.GetWarehouseStock;
public sealed class GetWarehouseStockQueryValidator : AbstractValidator<GetWarehouseStockQuery>
{
    public GetWarehouseStockQueryValidator() => RuleFor(x => x.WarehouseId).NotEmpty();
}
