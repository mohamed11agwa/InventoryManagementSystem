using Inventory.Application.Features.Products.Commands.CreateProduct;
using Inventory.Application.Features.Products.Commands.UpdateProduct;
using Inventory.Application.Features.Products.Queries.GetProductById;
using Inventory.Application.Features.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[Route("api/products")]
[Authorize]
public sealed class ProductsController(ISender sender) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetProductsQuery query, CancellationToken ct)
        => (await sender.Send(query, ct)).Match(Ok, Problem);

    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetById(Guid productId, CancellationToken ct)
        => (await sender.Send(new GetProductByIdQuery(productId), ct)).Match(Ok, Problem);

    [HttpPost]
    [Authorize(Policy = "AdministratorOnly")]
    public async Task<IActionResult> Create(CreateProductCommand command, CancellationToken ct)
        => (await sender.Send(command, ct)).Match(response => CreatedAtAction(nameof(GetById), new { productId = response.Id }, response), Problem);

    [HttpPut("{productId:guid}")]
    [Authorize(Policy = "AdministratorOnly")]
    public async Task<IActionResult> Update(Guid productId, UpdateProductCommand command, CancellationToken ct)
    {
        var updateCommand = new UpdateProductCommand(productId, command.Name, command.UnitPrice, command.CategoryId);
        return (await sender.Send(updateCommand, ct)).Match(Ok, Problem);
    }
}