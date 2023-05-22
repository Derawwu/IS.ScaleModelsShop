namespace IS.ScaleModelsShop.Application.Features.Products.Queries.GetAllProductsPaginated;

public class GetProductDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; } = default!;
}