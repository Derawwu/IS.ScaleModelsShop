using AutoMapper;
using IS.ScaleModelsShop.API.Contracts.Product.GetProduct;
using IS.ScaleModelsShop.Application.Exceptions;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductsByCategory;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using LinqKit;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductByManufacturer;

public class
    GetProductsByManufacturerQueryHandler : IRequestHandler<GetProductsByManufacturerQuery, IEnumerable<ProductByManufacturerModel>>
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public GetProductsByManufacturerQueryHandler(IMapper mapper, IProductRepository productRepository,
        IManufacturerRepository manufacturerRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _manufacturerRepository =
            manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
    }

    public async Task<IEnumerable<ProductByManufacturerModel>> Handle(GetProductsByManufacturerQuery request,
        CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        if (!await _manufacturerRepository.AnyAsync(x => x.Id == request.ManufacturerId))
            throw new NotFoundException(nameof(Manufacturer), request.ManufacturerId);

        var predicate = PredicateBuilder.New<Product>(true);
        predicate = predicate.And(x => x.ManufacturerId == request.ManufacturerId);

        var productList = await _productRepository.FilterAsync(predicate, x => x);

        if (productList.Any())
        {
            var result = _mapper.Map<IEnumerable<ProductByManufacturerModel>>(productList);

            return result;
        }

        return Array.Empty<ProductByManufacturerModel>();
    }
}