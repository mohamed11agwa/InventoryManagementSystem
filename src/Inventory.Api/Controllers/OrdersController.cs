using Inventory.Application.Features.Orders.Commands.CancelOrder;
using Inventory.Application.Features.Orders.Commands.CompleteOrder;
using Inventory.Application.Features.Orders.Commands.ConfirmOrder;
using Inventory.Application.Features.Orders.Commands.CreateOrder;
using Inventory.Application.Features.Orders.Commands.ProcessOrder;
using Inventory.Application.Features.Orders.Queries.GetOrderById;
using Inventory.Application.Features.Orders.Queries.GetOrders;
using Inventory.Application.Features.Orders.Queries.GetOrderHistory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[Route("api/orders")]
[Authorize]
public sealed class OrdersController(ISender sender) : ApiController
{
    [HttpGet]
    [Authorize(Policy = "OrderRead")]
    public async Task<IActionResult> Get([FromQuery] GetOrdersQuery query, CancellationToken ct)
        => (await sender.Send(query, ct)).Match(Ok, Problem);

    [HttpGet("{orderId:guid}")]
    [Authorize(Policy = "OrderRead")]
    public async Task<IActionResult> GetById(Guid orderId, CancellationToken ct)
        => (await sender.Send(new GetOrderByIdQuery(orderId), ct)).Match(Ok, Problem);

    [HttpGet("{orderId:guid}/history")]
    [Authorize(Policy = "OrderRead")]
    public async Task<IActionResult> GetHistory(Guid orderId, CancellationToken ct)
        => (await sender.Send(new GetOrderHistoryQuery(orderId), ct)).Match(Ok, Problem);

    [HttpPost]
    [Authorize(Policy = "SalesOrderManagement")]
    public async Task<IActionResult> Create(CreateOrderCommand command, [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey, CancellationToken ct)
    {
        if (!string.IsNullOrWhiteSpace(idempotencyKey))
            command = command with { IdempotencyKey = idempotencyKey };

        return (await sender.Send(command, ct)).Match(response => CreatedAtAction(nameof(GetById), new { orderId = response.Id }, response), Problem);
    }

    [HttpPost("{orderId:guid}/confirm")]
    [Authorize(Policy = "SalesOrderManagement")]
    public async Task<IActionResult> Confirm(Guid orderId, CancellationToken ct)
        => (await sender.Send(new ConfirmOrderCommand(orderId), ct)).Match(Ok, Problem);

    [HttpPost("{orderId:guid}/process")]
    [Authorize(Policy = "WarehouseOrderProcessing")]
    public async Task<IActionResult> Process(Guid orderId, CancellationToken ct)
        => (await sender.Send(new ProcessOrderCommand(orderId), ct)).Match(Ok, Problem);

    [HttpPost("{orderId:guid}/complete")]
    [Authorize(Policy = "WarehouseOrderProcessing")]
    public async Task<IActionResult> Complete(Guid orderId, CancellationToken ct)
        => (await sender.Send(new CompleteOrderCommand(orderId), ct)).Match(Ok, Problem);

    [HttpPost("{orderId:guid}/cancel")]
    [Authorize(Policy = "OrderCancellation")]
    public async Task<IActionResult> Cancel(Guid orderId, CancellationToken ct)
        => (await sender.Send(new CancelOrderCommand(orderId), ct)).Match(Ok, Problem);
}
