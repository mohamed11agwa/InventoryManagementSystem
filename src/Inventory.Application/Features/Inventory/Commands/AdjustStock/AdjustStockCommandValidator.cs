using FluentValidation;
namespace Inventory.Application.Features.Inventory.Commands.AdjustStock;
public sealed class AdjustStockCommandValidator : AbstractValidator<AdjustStockCommand>
{
    public AdjustStockCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.QuantityChange).NotEqual(0);
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
    }
}
