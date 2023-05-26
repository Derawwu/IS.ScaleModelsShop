namespace IS.ScaleModelsShop.API.Contracts.Product.GetProduct;

public class ProductBaseModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }
}