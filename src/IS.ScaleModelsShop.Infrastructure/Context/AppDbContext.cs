using IS.ScaleModelsShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IS.ScaleModelsShop.Infrastructure.Context;

/// <summary>
/// A context for Scale Model Shop API DB.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of <see cref="AppDbContext"/>
    /// </summary>
    /// <param name="options">The options.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets <see cref="DbSet{Product}" />.
    /// </summary>
    public virtual DbSet<Product> Products { get; set; }

    /// <summary>
    /// Gets or sets <see cref="DbSet{Category}" />.
    /// </summary>
    public virtual DbSet<Category> Categories { get; set; }

    /// <summary>
    /// Gets or sets <see cref="DbSet{Manufacturer}" />.
    /// </summary>
    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    /// <summary>
    /// Gets or sets <see cref="DbSet{ProductCategory}" />.
    /// </summary>
    public virtual DbSet<ProductCategory> ProductCategory { get; set; }

    /// <summary>
    /// Defines models structures.
    /// </summary>
    /// <param name="builder">Model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Product).Assembly);
    }
}