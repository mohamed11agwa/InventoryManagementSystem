using Inventory.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations;

public sealed class StockAdjustmentConfiguration : IEntityTypeConfiguration<StockAdjustment>
{
    public void Configure(EntityTypeBuilder<StockAdjustment> builder)
    {
        builder.ToTable("StockAdjustments");
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        builder.Property(x => x.InventoryId).IsRequired();
        builder.Property(x => x.QuantityChange).IsRequired();
        builder.Property(x => x.PreviousQuantity).IsRequired();
        builder.Property(x => x.NewQuantity).IsRequired();
        builder.Property(x => x.AdjustedAtUtc).IsRequired();
        builder.Property(x => x.AdjustedBy).HasMaxLength(450).IsRequired();

        builder.Property(x => x.Reason).HasMaxLength(500).IsRequired();
       
        builder.HasIndex(x => x.AdjustedAtUtc);
        builder.HasIndex(x => x.InventoryId);
    }
}
