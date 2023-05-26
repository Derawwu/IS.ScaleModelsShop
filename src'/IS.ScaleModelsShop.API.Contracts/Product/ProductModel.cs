namespace IS.ScaleModelsShop.API.Contracts.Product
{
    public class ProductModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public Guid ManufacturerId { get; set; }

        public Guid CategoryId { get; set; }
    }
}
