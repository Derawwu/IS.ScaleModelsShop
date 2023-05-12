using IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductsByCategory;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductByManufacturer
{
    public class GetProductsByManufacturerQuery : IRequest<IEnumerable<ProductsDTO>>
    {
        public Guid ManufacturerId { get; set; } = Guid.Empty;
    }
}