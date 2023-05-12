using IS.ScaleModelsShop.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace IS.ScaleModelsShop.Domain.Entities
{
    public class Category : AuditableEntity
    {


        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<ProductCategory> ProductCategory { get; set; }
    }
}