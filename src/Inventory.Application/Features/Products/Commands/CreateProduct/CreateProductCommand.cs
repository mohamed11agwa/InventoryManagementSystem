using Inventory.Application.Features.Products.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;

namespace Inventory.Application.Features.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(string Name, decimal UnitPrice, Guid? CategoryId) : IRequest<Result<ProductDto>>;
