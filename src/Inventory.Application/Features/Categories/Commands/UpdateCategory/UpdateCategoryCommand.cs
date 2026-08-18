using Inventory.Application.Features.Categories.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;
namespace Inventory.Application.Features.Categories.Commands.UpdateCategory;
public sealed record UpdateCategoryCommand(Guid Id, string Name) : IRequest<Result<CategoryDto>>;
