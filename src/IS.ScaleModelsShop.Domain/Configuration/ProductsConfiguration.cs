using IS.ScaleModelsShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IS.ScaleModelsShop.Domain.Configuration;

/// <summary>
/// Class with configuration for SQL table containing Product entities.
/// </summary>
public class ProductsConfiguration : IEntityTypeConfiguration<Product>
{
    /// <summary>
    /// Configures SQL table containing Product entities.
    /// </summary>
    /// <param name="builder">A <see cref="EntityTypeBuilder"/>.</param>
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasPrecision(4, 2);

        builder.HasOne(p => p.Manufacturer)
            .WithMany(m => m.Products)
            .HasForeignKey(p => p.ManufacturerId);

        builder.Property(p => p.CategoryId)
            .IsRequired();

        builder.HasMany(p => p.ProductCategory)
            .WithOne(pc => pc.Product)
            .HasForeignKey(pc => pc.LinkedProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}