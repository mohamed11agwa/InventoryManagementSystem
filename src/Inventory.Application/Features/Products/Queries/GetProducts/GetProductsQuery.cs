using Inventory.Application.Features.Products.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;

namespace Inventory.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductsQuery : IRequest<Result<List<ProductDto>>>;
