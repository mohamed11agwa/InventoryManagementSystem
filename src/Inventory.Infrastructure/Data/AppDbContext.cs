using Inventory.Application.Common.Interfaces;
using Inventory.Domain.Categories;
using Inventory.Domain.Common;
using Inventory.Domain.Customers;
using Inventory.Domain.Inventory;
using Inventory.Domain.Orders;
using Inventory.Domain.Products;
using Inventory.Domain.Warehouses;
using Inventory.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Inventory.Infrastructure.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator)
    : IdentityDbContext<AppUser>(options), IAppDbContext
{
    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<Inventory.Domain.Inventory.Inventory> Inventories => Set<Inventory.Domain.Inventory.Inventory>();
    public DbSet<StockAdjustment> StockAdjustments => Set<StockAdjustment>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderStatusHistory> OrderStatusHistories => Set<OrderStatusHistory>();
    public DatabaseFacade Database => base.Database;

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync(cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEntities = ChangeTracker.Entries()
            .Where(e => e.Entity is Entity entity && entity.DomainEvents.Count != 0)
            .Select(e => (Entity)e.Entity)
            .ToList();

        var domainEvents = domainEntities.SelectMany(e => e.DomainEvents).ToList();

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }

        foreach (var entity in domainEntities)
        {
            entity.ClearDomainEvents();
        }
    }
}
