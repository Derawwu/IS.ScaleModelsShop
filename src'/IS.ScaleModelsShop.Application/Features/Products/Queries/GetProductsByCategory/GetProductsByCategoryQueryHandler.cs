using AutoMapper;
using IS.ScaleModelsShop.Application.Exceptions;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using LinqKit;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductsByCategory;

public class GetProductsByCategoryQueryHandler : IRequestHandler<GetProductsByCategoryQuery, IEnumerable<ProductsDTO>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public GetProductsByCategoryQueryHandler(IMapper mapper, IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    public async Task<IEnumerable<ProductsDTO>> Handle(GetProductsByCategoryQuery request,
        CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        if (!await _categoryRepository.AnyAsync(x => x.Id == request.CategoryId))
            throw new NotFoundException(nameof(Category), request.CategoryId);

        var predicate = PredicateBuilder.New<Product>(true);

        predicate = predicate.And(x => x.ProductCategory.Any(y => y.Category.Id == request.CategoryId));

        var productList = await _productRepository.FilterAsync(predicate, x => x);

        if (productList.Any())
        {
            var result = _mapper.Map<IEnumerable<ProductsDTO>>(productList);

            return result;
        }

        return Enumerable.Empty<ProductsDTO>();
    }
}