using IS.ScaleModelsShop.Domain.Common;

namespace IS.ScaleModelsShop.Infrastructure.UnitTests.FakeEntities
{
    public class FakeAuditableEntity : AuditableEntity
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }
    }
}