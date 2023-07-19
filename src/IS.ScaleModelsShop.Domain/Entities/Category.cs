using System.ComponentModel.DataAnnotations.Schema;
using IS.ScaleModelsShop.Domain.Common;

namespace IS.ScaleModelsShop.Domain.Entities;

/// <summary>
/// Data model for Category.
/// </summary>
public class Category : AuditableEntity
{
    /// <summary>
    /// Gets or sets Name of the Category.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets ProductCategory of the Category.
    /// </summary>
    [InverseProperty("Category")]
    public virtual ICollection<ProductCategory> ProductCategory { get; set; }
}