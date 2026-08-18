using Inventory.Application.Features.Products.Dtos;
using Inventory.Domain.Products;

namespace Inventory.Application.Features.Products.Mappers;

public static class ProductMapper
{
    public static ProductDto ToDto(this Product product)
        => new(product.Id, product.Name, product.CategoryId);

    public static List<ProductDto> ToDtos(this IEnumerable<Product> products)
        => products.Select(product => product.ToDto()).ToList();
}
