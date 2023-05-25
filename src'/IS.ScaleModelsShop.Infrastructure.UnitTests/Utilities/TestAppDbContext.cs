using IS.ScaleModelsShop.Infrastructure.Context;
using IS.ScaleModelsShop.Infrastructure.UnitTests.FakeEntities;
using Microsoft.EntityFrameworkCore;

namespace IS.ScaleModelsShop.Infrastructure.UnitTests.Utilities
{
    public class TestAppDbContext : AppDbContext
    {
        public TestAppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<FakeEntity> FakeEntities { get; set; }
        public DbSet<FakeAuditableEntity> FakeAuditableEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.ApplyConfiguration(new FakeEntityDataConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
