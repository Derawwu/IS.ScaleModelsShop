namespace IS.ScaleModelsShop.API.Contracts.Product.GetProduct
{
    public class ProductByCategoryModel : Product
    {
        public Guid ManufacturerId { get; set; }
    }
}