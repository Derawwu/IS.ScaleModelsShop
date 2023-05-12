using IS.ScaleModelsShop.Domain.Common;

namespace IS.ScaleModelsShop.Domain.Entities
{
    public class Manufacturer : AuditableEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Website { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}