using Inventory.Application.Features.Warehouses.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;
namespace Inventory.Application.Features.Warehouses.Queries.GetWarehouses;
public sealed record GetWarehousesQuery : IRequest<Result<List<WarehouseDto>>>;
