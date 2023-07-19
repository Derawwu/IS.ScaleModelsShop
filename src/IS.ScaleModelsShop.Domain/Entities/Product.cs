using System.ComponentModel.DataAnnotations.Schema;
using IS.ScaleModelsShop.Domain.Common;

namespace IS.ScaleModelsShop.Domain.Entities;

/// <summary>
/// Data model for the Product.
/// </summary>
public class Product : AuditableEntity
{
    /// <summary>
    /// Gets or sets Name of the Product.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets Description of the Product.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets Price of the Product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets ManufacturerId of the Product.
    /// </summary>
    public Guid ManufacturerId { get; set; }

    /// <summary>
    /// Gets or sets Manufacturer of the Product.
    /// </summary>
    public Manufacturer Manufacturer { get; set; }

    /// <summary>
    /// Gets or sets CategoryId of the Product.
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Gets or sets Category of the Product.
    /// </summary>
    public Category Category { get; set; }

    /// <summary>
    /// Gets or sets ProductCategory of the Product.
    /// </summary>
    [InverseProperty("Product")]
    public virtual ICollection<ProductCategory> ProductCategory { get; set; }
}