namespace IS.ScaleModelsShop.API.Contracts.Product.CreateProduct
{
    public class CreateProductModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public Guid ManufacturerId { get; set; }

        public Guid CategoryId { get; set; }

        public override string ToString()
        {
            return $"Product name: {Name}; Price: {Price}; Description: {Description}";
        }
    }
}