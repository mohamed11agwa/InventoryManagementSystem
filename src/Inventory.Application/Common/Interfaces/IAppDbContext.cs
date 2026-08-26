using Inventory.Domain.Categories;
using Inventory.Domain.Customers;
using Inventory.Domain.Inventory;
using Inventory.Domain.Orders;
using Inventory.Domain.Products;
using Inventory.Domain.Warehouses;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Inventory.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<Customer> Customers { get; }
    DbSet<Category> Categories { get; }
    DbSet<Product> Products { get; }
    DbSet<Warehouse> Warehouses { get; }
    DbSet<Inventory.Domain.Inventory.Inventory> Inventories { get; }
    DbSet<StockAdjustment> StockAdjustments { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<OrderStatusHistory> OrderStatusHistories { get; }
    DatabaseFacade Database { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
