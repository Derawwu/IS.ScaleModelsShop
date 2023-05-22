using AutoMapper;
using IS.ScaleModelsShop.Application.Repositories;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Queries.GetAllProductsPaginated
{
    public class GetAllProductsPaginatedQueryHandler : IRequestHandler<GetAllProductsPaginatedQuery, GetPaginatedProductViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public GetAllProductsPaginatedQueryHandler(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<GetPaginatedProductViewModel> Handle(GetAllProductsPaginatedQuery request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            var list = await _productRepository.GetPaginatedProductsAsync(request.PageNumber, request.PageSize);
            var products = _mapper.Map<List<GetProductDTO>>(list);

            return new GetPaginatedProductViewModel
                { Page = request.PageNumber, Size = request.PageSize, Products = products };
        }
    }
}