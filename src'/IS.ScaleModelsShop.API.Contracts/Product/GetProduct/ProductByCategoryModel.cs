namespace IS.ScaleModelsShop.API.Contracts.Product.GetProduct;

public class ProductByCategoryModel : ProductBaseModel
{
    public Guid ManufacturerId { get; set; }
}