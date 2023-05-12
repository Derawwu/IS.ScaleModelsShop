using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByNameQuery : IRequest<GetProductByNameViewModel>
    {
        public string Name { get; set; } = string.Empty;
    }
}