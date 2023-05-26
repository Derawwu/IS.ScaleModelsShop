using AutoMapper;
using IS.ScaleModelsShop.API.Contracts.Product;
using IS.ScaleModelsShop.Application.Exceptions;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductByName;

public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, ProductModel>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public GetProductByNameQueryHandler(IMapper mapper, IProductRepository productRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<ProductModel> Handle(GetProductByNameQuery request,
        CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var product = await _productRepository.GetEntityByNameAsync(p => p.Name == request.Name);

        if (product == null) throw new NotFoundException(nameof(Product), request.Name);

        var productToReturn = _mapper.Map<ProductModel>(product);

        return productToReturn;
    }
}