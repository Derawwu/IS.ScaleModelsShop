namespace IS.ScaleModelsShop.Domain.Common;

/// <summary>
/// Abstraction of a persisted entity which has auditable information about when it was created, when it was last modified.
/// </summary>
public class AuditableEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets user who created an entity.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets user who last modified an entity.
    /// </summary>
    public string? LastModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the last modified date.
    /// </summary>
    public DateTime? LastModifiedDate { get; set; }
}