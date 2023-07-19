using System.ComponentModel.DataAnnotations.Schema;
using IS.ScaleModelsShop.Domain.Common;

namespace IS.ScaleModelsShop.Domain.Entities;

/// <summary>
/// Data model for ProductCategory.
/// </summary>
public class ProductCategory : AuditableEntity
{
    /// <summary>
    /// Gets or sets LinkedCategoryId of the ProductCategory.
    /// </summary>
    public Guid LinkedCategoryId { get; set; }

    /// <summary>
    /// Gets or sets LinkedProductId of the ProductCategory.
    /// </summary>
    public Guid LinkedProductId { get; set; }

    /// <summary>
    /// Gets or sets Product of the ProductCategory.
    /// </summary>
    [InverseProperty(nameof(ProductCategory))]
    public virtual Product Product { get; set; }

    /// <summary>
    /// Gets or sets Category of the ProductCategory.
    /// </summary>
    [InverseProperty(nameof(ProductCategory))]
    public virtual Category Category { get; set; }
}