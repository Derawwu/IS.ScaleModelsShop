using IS.ScaleModelsShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IS.ScaleModelsShop.Domain.Configuration;

/// <summary>
/// Class with configuration for SQL table containing Manufacturer entities.
/// </summary>
public class ManufacturerConfiguration : IEntityTypeConfiguration<Manufacturer>
{
    /// <summary>
    /// Configures SQL table containing Manufacturer entities.
    /// </summary>
    /// <param name="builder">A <see cref="EntityTypeBuilder"/>.</param>
    public void Configure(EntityTypeBuilder<Manufacturer> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedOnAdd();

        builder.HasMany(m => m.Products)
            .WithOne(p => p.Manufacturer)
            .HasForeignKey(p => p.ManufacturerId)
            .IsRequired();

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.Website)
            .HasMaxLength(40);
    }
}