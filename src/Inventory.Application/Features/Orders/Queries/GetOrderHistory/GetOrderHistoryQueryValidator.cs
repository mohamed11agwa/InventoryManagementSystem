using FluentValidation;

namespace Inventory.Application.Features.Orders.Queries.GetOrderHistory;

public sealed class GetOrderHistoryQueryValidator : AbstractValidator<GetOrderHistoryQuery>
{
    public GetOrderHistoryQueryValidator() => RuleFor(x => x.OrderId).NotEmpty();
}
