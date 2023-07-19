namespace IS.ScaleModelsShop.API.Contracts.Product.UpdateProduct;

/// <summary>
/// Model for the update of the Product.
/// </summary>
public class UpdateProductModel
{
    /// <summary>
    /// Name of the Product to Update to.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of the Product to Update to.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Price of the Product to Update to.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// ManufacturerId of the Product to Update to.
    /// </summary>
    public Guid ManufacturerId { get; set; }

    /// <summary>
    /// CategoryId of the Product to Update to.
    /// </summary>
    public Guid CategoryId { get; set; }
}