namespace IS.ScaleModelsShop.API.Contracts.Category;

/// <summary>
/// Model for mapping between infrastructure and application logic.
/// </summary>
public class CategoryModel
{
    /// <summary>
    /// Gets or sets Guid of the category.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets name of the category.
    /// </summary>
    public string Name { get; set; }
}