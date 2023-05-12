using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductsByCategory
{
    public class GetProductsByCategoryQuery : IRequest<IEnumerable<ProductsDTO>>
    {
        public Guid CategoryId { get; set; } = Guid.Empty;
    }
}