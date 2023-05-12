namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Queries.GetManufacturerProducts
{
    public class ManufacturerProductsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<ManufacturerProductsDTO>? Products { get; set; }
    }
}