using Inventory.Application.Common.Interfaces;
using Inventory.Domain.Orders;
using MediatR;

namespace Inventory.Application.Features.Orders.EventHandlers;

public sealed class OrderStatusChangedEventHandler(IAppDbContext context)
    : INotificationHandler<OrderStatusChangedEvent>
{
    public Task Handle(OrderStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        context.OrderStatusHistories.Add(new OrderStatusHistory(
            Guid.NewGuid(),
            notification.OrderId,
            notification.From,
            notification.To,
            notification.ChangedAtUtc,
            notification.ChangedBy));

        return Task.CompletedTask;
    }
}
