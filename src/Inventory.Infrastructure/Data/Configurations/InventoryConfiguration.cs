using Inventory.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations;

public sealed class InventoryConfiguration : IEntityTypeConfiguration<Inventory.Domain.Inventory.Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory.Domain.Inventory.Inventory> builder)
    {
        builder.ToTable("Inventories");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.WarehouseId).IsRequired();
        builder.Property(x => x.Quantity).IsRequired();
        builder.HasIndex(x => new { x.ProductId, x.WarehouseId }).IsUnique();
        builder.HasIndex(x => x.WarehouseId);
        builder.Property(x => x.CreatedBy).HasMaxLength(450);
        builder.Property(x => x.LastModifiedBy).HasMaxLength(450);

        builder.HasMany<StockAdjustment>("_adjustments")
            .WithOne()
            .HasForeignKey(x => x.InventoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation("_adjustments").UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
