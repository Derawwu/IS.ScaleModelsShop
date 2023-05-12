namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Queries.GetManufacturerProducts
{
    public class ManufacturerProductsDTO
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string ProductDescription { get; set; } = string.Empty;

        public int Price { get; set; }

        public string Manufacturer { get; set; } = string.Empty;
    }
}