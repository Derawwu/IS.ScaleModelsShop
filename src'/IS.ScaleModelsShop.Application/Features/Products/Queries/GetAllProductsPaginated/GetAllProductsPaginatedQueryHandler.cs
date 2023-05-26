using AutoMapper;
using IS.ScaleModelsShop.API.Contracts.Product;
using IS.ScaleModelsShop.API.Contracts.Product.GetProduct;
using IS.ScaleModelsShop.Application.Repositories;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Queries.GetAllProductsPaginated;

public class
    GetAllProductsPaginatedQueryHandler : IRequestHandler<GetAllProductsPaginatedQuery, PaginatedProductsModel>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public GetAllProductsPaginatedQueryHandler(IMapper mapper, IProductRepository productRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<PaginatedProductsModel> Handle(GetAllProductsPaginatedQuery request,
        CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var list = await _productRepository.GetPaginatedProductsAsync(request.PageNumber, request.PageSize);
        var products = _mapper.Map<List<ProductModel>>(list);

        return new PaginatedProductsModel
        { Page = request.PageNumber, Size = request.PageSize, Products = products };
    }
}