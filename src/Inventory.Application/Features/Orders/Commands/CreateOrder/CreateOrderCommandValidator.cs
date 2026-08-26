using FluentValidation;

namespace Inventory.Application.Features.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.IdempotencyKey).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Items).NotEmpty().Must(items => items.Count <= 100);
        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId).NotEmpty();
            item.RuleFor(x => x.WarehouseId).NotEmpty();
            item.RuleFor(x => x.Quantity).GreaterThan(0);
        });

        RuleFor(x => x.Items)
            .Must(items => items.Select(i => (i.ProductId, i.WarehouseId)).Distinct().Count() == items.Count)
            .WithMessage("The same product cannot be added more than once for the same warehouse.");
    }
}
