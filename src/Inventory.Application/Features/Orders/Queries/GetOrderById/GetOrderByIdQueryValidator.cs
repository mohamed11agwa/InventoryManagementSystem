using FluentValidation;

namespace Inventory.Application.Features.Orders.Queries.GetOrderById;

public sealed class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdQueryValidator() => RuleFor(x => x.OrderId).NotEmpty();
}
