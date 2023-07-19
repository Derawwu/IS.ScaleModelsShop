using IS.ScaleModelsShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IS.ScaleModelsShop.Domain.Configuration;

/// <summary>
/// Class with configuration for SQL table containing Category entities.
/// </summary>
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    /// <summary>
    /// Configures SQL table containing Category entities.
    /// </summary>
    /// <param name="builder">A <see cref="EntityTypeBuilder"/>.</param>
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}