using Inventory.Application.Features.Categories.Commands.CreateCategory;
using Inventory.Application.Features.Categories.Commands.UpdateCategory;
using Inventory.Application.Features.Categories.Queries.GetCategories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[Route("api/categories")]
[Authorize]
public sealed class CategoriesController(ISender sender) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
        => (await sender.Send(new GetCategoriesQuery(), ct)).Match(Ok, Problem);

    [HttpPost]
    [Authorize(Policy = "AdministratorOnly")]
    public async Task<IActionResult> Create(CreateCategoryCommand command, CancellationToken ct)
    {
        return (await sender.Send(command, ct)).Match(Ok, Problem);

    }


    [HttpPut("{categoryId:guid}")]
    [Authorize(Policy = "AdministratorOnly")]
    public async Task<IActionResult> Update(Guid categoryId,  UpdateCategoryCommand command, CancellationToken ct)
    {
        var updateCommand = new UpdateCategoryCommand(categoryId, command.Name);

        return (await sender.Send(updateCommand, ct)).Match(Ok, Problem);
    }
}
