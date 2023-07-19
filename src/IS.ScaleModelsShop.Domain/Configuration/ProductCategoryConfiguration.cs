using IS.ScaleModelsShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IS.ScaleModelsShop.Domain.Configuration
{
    /// <summary>
    /// Class with configuration for SQL table containing ProductCategory entities.
    /// </summary>
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        /// <summary>
        /// Configures SQL table containing ProductCategory entities.
        /// </summary>
        /// <param name="builder">A <see cref="EntityTypeBuilder"/>.</param>
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {

            builder.HasKey(pc => pc.Id);

            builder.Property(pc => pc.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategory)
                .HasForeignKey(pc => pc.LinkedProductId);

            builder.HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategory)
                .HasForeignKey(pc => pc.LinkedCategoryId);
        }
    }
}
