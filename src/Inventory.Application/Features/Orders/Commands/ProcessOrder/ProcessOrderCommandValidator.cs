using FluentValidation;

namespace Inventory.Application.Features.Orders.Commands.ProcessOrder;

public sealed class ProcessOrderCommandValidator : AbstractValidator<ProcessOrderCommand>
{
    public ProcessOrderCommandValidator() => RuleFor(x => x.OrderId).NotEmpty();
}
