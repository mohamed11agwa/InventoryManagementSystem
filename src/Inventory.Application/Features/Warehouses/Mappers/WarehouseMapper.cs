using Inventory.Application.Features.Warehouses.Dtos;
using Inventory.Domain.Warehouses;
namespace Inventory.Application.Features.Warehouses.Mappers;
public static class WarehouseMapper
{
    public static WarehouseDto ToDto(this Warehouse warehouse) => new(warehouse.Id, warehouse.Name);
}
