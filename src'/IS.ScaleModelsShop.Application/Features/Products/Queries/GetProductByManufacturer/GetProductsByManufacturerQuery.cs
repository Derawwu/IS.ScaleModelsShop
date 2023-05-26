using IS.ScaleModelsShop.API.Contracts.Product.GetProduct;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductByManufacturer;

public class GetProductsByManufacturerQuery : IRequest<IEnumerable<ProductByManufacturerModel>>
{
    public Guid ManufacturerId { get; set; } = Guid.Empty;
}