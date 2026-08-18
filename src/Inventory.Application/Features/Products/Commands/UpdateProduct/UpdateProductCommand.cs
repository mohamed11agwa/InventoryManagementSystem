using Inventory.Application.Features.Products.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;

namespace Inventory.Application.Features.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(Guid Id, string Name, Guid? CategoryId) : IRequest<Result<ProductDto>>;
