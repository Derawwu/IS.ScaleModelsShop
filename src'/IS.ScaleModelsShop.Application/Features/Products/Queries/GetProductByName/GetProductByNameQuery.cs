using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductByName;

public class GetProductByNameQuery : IRequest<GetProductByNameViewModel>
{
    public string Name { get; set; } = string.Empty;
}