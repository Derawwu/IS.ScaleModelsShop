using IS.ScaleModelsShop.API.Contracts.Product.GetProduct;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Queries.GetAllProductsPaginated;

public class GetAllProductsPaginatedQuery : IRequest<PaginatedProductsModel>
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }
}