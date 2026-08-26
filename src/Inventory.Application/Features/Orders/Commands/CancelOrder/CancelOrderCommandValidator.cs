using FluentValidation;

namespace Inventory.Application.Features.Orders.Commands.CancelOrder;

public sealed class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator() => RuleFor(x => x.OrderId).NotEmpty();
}
