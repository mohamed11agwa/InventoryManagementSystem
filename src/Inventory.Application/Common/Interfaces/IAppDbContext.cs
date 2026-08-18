using Inventory.Domain.Categories;
using Inventory.Domain.Inventory;
using Inventory.Domain.Products;
using Inventory.Domain.Warehouses;

using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<Category> Categories { get; }
    DbSet<Product> Products { get; }
    DbSet<Warehouse> Warehouses { get; }
    DbSet<Inventory.Domain.Inventory.Inventory> Inventories { get; }
    DbSet<StockAdjustment> StockAdjustments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
