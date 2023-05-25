namespace IS.ScaleModelsShop.Infrastructure.UnitTests.FakeEntities
{
    public class FakeEntity : FakeAuditableEntity
    {
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}