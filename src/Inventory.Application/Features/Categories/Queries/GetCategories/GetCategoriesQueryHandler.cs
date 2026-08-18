using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Categories.Dtos;
using Inventory.Application.Features.Categories.Mappers;
using Inventory.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Inventory.Application.Features.Categories.Queries.GetCategories;
public sealed class GetCategoriesQueryHandler(IAppDbContext context) : IRequestHandler<GetCategoriesQuery, Result<List<CategoryDto>>>
{
    public async Task<Result<List<CategoryDto>>> Handle(GetCategoriesQuery query, CancellationToken ct)
    {
        var categories = await context.Categories.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct);
        return categories.Select(x => x.ToDto()).ToList();
    }
}
