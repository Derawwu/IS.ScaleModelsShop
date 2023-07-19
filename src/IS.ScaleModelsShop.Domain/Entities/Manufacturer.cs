using IS.ScaleModelsShop.Domain.Common;

namespace IS.ScaleModelsShop.Domain.Entities;

/// <summary>
/// Data model for the Manufacturer.
/// </summary>
public class Manufacturer : AuditableEntity
{
    /// <summary>
    /// Gets or sets Name of the Manufacturer.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets Website of the Manufacturer.
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// Gets or sets Products of the Manufacturer.
    /// </summary>
    public ICollection<Product> Products { get; set; }
}