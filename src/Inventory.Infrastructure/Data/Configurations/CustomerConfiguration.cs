using Inventory.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(320).IsRequired();
        builder.HasIndex(x => x.Email);
        builder.Property(x => x.CreatedBy).HasMaxLength(450);
        builder.Property(x => x.LastModifiedBy).HasMaxLength(450);
    }
}
