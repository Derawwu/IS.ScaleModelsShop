using IS.ScaleModelsShop.Infrastructure.UnitTests.FakeEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IS.ScaleModelsShop.Infrastructure.UnitTests.Utilities
{
    public class FakeEntityDataConfiguration : IEntityTypeConfiguration<FakeEntity>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<FakeEntity> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
        }
    }
}