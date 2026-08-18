using Inventory.Application.Features.Categories.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;
namespace Inventory.Application.Features.Categories.Queries.GetCategories;
public sealed record GetCategoriesQuery : IRequest<Result<List<CategoryDto>>>;
