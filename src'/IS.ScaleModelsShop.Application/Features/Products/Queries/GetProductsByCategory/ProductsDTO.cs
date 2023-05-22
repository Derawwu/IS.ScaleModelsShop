namespace IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductsByCategory;

public class ProductsDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public Guid Manufacturer { get; set; } = Guid.Empty;
}