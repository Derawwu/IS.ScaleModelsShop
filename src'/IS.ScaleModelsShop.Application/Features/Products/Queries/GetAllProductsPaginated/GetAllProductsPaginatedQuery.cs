using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Queries.GetAllProductsPaginated;

public class GetAllProductsPaginatedQuery : IRequest<GetPaginatedProductViewModel>
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }
}