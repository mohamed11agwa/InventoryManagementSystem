using Inventory.Application.Features.Categories.Dtos;
using Inventory.Domain.Categories;
namespace Inventory.Application.Features.Categories.Mappers;
public static class CategoryMapper
{
    public static CategoryDto ToDto(this Category category) => new(category.Id, category.Name);
}
