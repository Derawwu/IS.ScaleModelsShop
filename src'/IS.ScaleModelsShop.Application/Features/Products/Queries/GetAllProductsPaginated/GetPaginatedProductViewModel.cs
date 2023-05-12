namespace IS.ScaleModelsShop.Application.Features.Products.Queries.GetAllProductsPaginated
{
    public class GetPaginatedProductViewModel
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public ICollection<GetProductDTO>? Products { get; set; }
    }
}