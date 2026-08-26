using Inventory.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", table =>
        {
            table.HasCheckConstraint(
                "CK_OrderItems_UnitPrice_NonNegative",
                "[UnitPrice] >= 0");
            table.HasCheckConstraint(
                "CK_OrderItems_Quantity_Positive",
                "[Quantity] > 0");
        });
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.WarehouseId).IsRequired();
        builder.Property(x => x.ProductName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.UnitPrice).HasPrecision(18, 2).IsRequired();
        builder.Property(x => x.Quantity).IsRequired();
        builder.Property(x => x.StockConsumed).IsRequired();
        builder.Property(x => x.StockRestored).IsRequired();
        builder.Ignore(x => x.LineTotal);
        builder.HasIndex(x => new { x.OrderId, x.ProductId, x.WarehouseId }).IsUnique();
        builder.HasOne<Inventory.Domain.Products.Product>()
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Inventory.Domain.Warehouses.Warehouse>()
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
