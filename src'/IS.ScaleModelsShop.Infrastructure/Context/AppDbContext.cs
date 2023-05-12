using IS.ScaleModelsShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IS.ScaleModelsShop.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);


            modelBuilder.Entity<Product>()
                .Property(p => p.Id)
                .HasDefaultValueSql("newId()");

            modelBuilder.Entity<Category>()
                .Property(c => c.Id)
                .HasDefaultValueSql("newId()");

            modelBuilder.Entity<Manufacturer>()
                .Property(m => m.Id)
                .HasDefaultValueSql("newId()");

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => new { e.LinkedCategoryId, e.LinkedProductId });

                entity.HasOne(x => x.Product)
                    .WithMany(x => x.ProductCategory)
                    .HasForeignKey(x => x.LinkedProductId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_ProductCategory_Products_LinkedProductId");

                entity.HasOne(x => x.Category)
                    .WithMany(x => x.ProductCategory)
                    .HasForeignKey(x => x.LinkedCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCategory_Category_LinkedCategoryId");
            });
        }
    }
}