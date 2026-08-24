using Inventory.Domain.Products;
using Inventory.Domain.Warehouses;
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

        builder.HasIndex(x => new { x.ProductId, x.WarehouseId }).IsUnique();
        builder.HasIndex(x => x.WarehouseId);

        builder.Property(x => x.Quantity).IsRequired();

        builder.ToTable("Inventories", table =>
        {
            table.HasCheckConstraint(
                "CK_Inventories_Quantity_NonNegative",
                "[Quantity] >= 0");
        });

        builder.HasOne<Product>()
               .WithMany()
               .HasForeignKey(x => x.ProductId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Warehouse>()
               .WithMany()
               .HasForeignKey(x => x.WarehouseId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Adjustments)
            .WithOne()
            .HasForeignKey(x => x.InventoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CreatedBy).HasMaxLength(450);
        builder.Property(x => x.LastModifiedBy).HasMaxLength(450);

        builder.Navigation(x => x.Adjustments)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property<byte[]>("RowVersion")
            .IsRowVersion();
    }
}
