namespace IS.ScaleModelsShop.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductDTO
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; } = default!;

        public Guid Manufacturer { get; set; } = Guid.Empty;

        public Guid Category { get; set; } = Guid.Empty;
    }
}