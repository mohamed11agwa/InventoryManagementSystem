using Inventory.Application.Features.Categories.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;

namespace Inventory.Application.Features.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(string Name) : IRequest<Result<CategoryDto>>;
