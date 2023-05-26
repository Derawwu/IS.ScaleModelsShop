using IS.ScaleModelsShop.API.Contracts.Product.GetProduct;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductsByCategory;

public class GetProductsByCategoryQuery : IRequest<IEnumerable<ProductByCategoryModel>>
{
    public Guid CategoryId { get; set; } = Guid.Empty;
}