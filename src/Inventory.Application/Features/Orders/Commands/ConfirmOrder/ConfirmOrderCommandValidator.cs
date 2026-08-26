using FluentValidation;

namespace Inventory.Application.Features.Orders.Commands.ConfirmOrder;

public sealed class ConfirmOrderCommandValidator : AbstractValidator<ConfirmOrderCommand>
{
    public ConfirmOrderCommandValidator() => RuleFor(x => x.OrderId).NotEmpty();
}
