using System.ComponentModel.DataAnnotations.Schema;
using IS.ScaleModelsShop.Domain.Common;

namespace IS.ScaleModelsShop.Domain.Entities;

public class ProductCategory : AuditableEntity
{
    public Guid Id { get; set; }

    public Guid LinkedCategoryId { get; set; }

    public Guid LinkedProductId { get; set; }

    [ForeignKey(nameof(LinkedProductId))]
    [InverseProperty(nameof(ProductCategory))]
    public virtual Product Product { get; set; }

    [ForeignKey(nameof(LinkedCategoryId))]
    [InverseProperty(nameof(ProductCategory))]
    public Category Category { get; set; }
}