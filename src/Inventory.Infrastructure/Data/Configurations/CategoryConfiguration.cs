using Inventory.Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
               .HasMaxLength(100)
               .IsRequired();
        builder.HasIndex(x => x.Name);

        ConfigureAudit(builder);
    }

    private static void ConfigureAudit(EntityTypeBuilder<Category> builder)
    {
        builder.Property(x => x.CreatedBy)
               .HasMaxLength(450);
        builder.Property(x => x.LastModifiedBy)
               .HasMaxLength(450);
    }
}
