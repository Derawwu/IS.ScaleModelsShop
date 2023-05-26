using IS.ScaleModelsShop.API.Contracts.Product;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductByName;

public class GetProductByNameQuery : IRequest<ProductModel>
{
    public string Name { get; set; } = string.Empty;
}