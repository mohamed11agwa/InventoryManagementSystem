using Inventory.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations;

public sealed class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
{
    public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
    {
        builder.ToTable("OrderStatusHistories");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FromStatus).HasConversion<string>().HasMaxLength(32).IsRequired();
        builder.Property(x => x.ToStatus).HasConversion<string>().HasMaxLength(32).IsRequired();
        builder.Property(x => x.ChangedAtUtc).IsRequired();
        builder.Property(x => x.ChangedBy).HasMaxLength(450).IsRequired();
        builder.HasIndex(x => new { x.OrderId, x.ChangedAtUtc });
        builder.HasOne<Order>()
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
