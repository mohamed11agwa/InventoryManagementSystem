using Inventory.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
        builder.Property(x => x.Total).HasPrecision(18, 2).IsRequired();
        builder.Property(x => x.IdempotencyKey).HasMaxLength(100).IsRequired();
        builder.HasIndex(x => x.IdempotencyKey).IsUnique();
        builder.Property<byte[]>("RowVersion").IsRowVersion();
        builder.Property(x => x.CreatedBy).HasMaxLength(450);
        builder.Property(x => x.LastModifiedBy).HasMaxLength(450);

        builder.HasOne<Inventory.Domain.Customers.Customer>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
