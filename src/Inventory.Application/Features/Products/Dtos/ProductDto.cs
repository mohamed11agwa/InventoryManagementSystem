namespace Inventory.Application.Features.Products.Dtos;

public sealed record ProductDto(Guid Id, string Name, Guid? CategoryId);
