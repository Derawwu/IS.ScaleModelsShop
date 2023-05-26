namespace IS.ScaleModelsShop.API.Contracts.Product.GetProduct
{
    public class PaginatedProductsModel
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public ICollection<ProductModel>? Products { get; set; }
    }
}