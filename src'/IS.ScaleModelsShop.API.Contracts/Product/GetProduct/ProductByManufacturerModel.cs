namespace IS.ScaleModelsShop.API.Contracts.Product.GetProduct
{
    public class ProductByManufacturerModel : Product
    {
        public Guid CategoryId { get; set; }
    }
}