using AutoMapper;
using IS.ScaleModelsShop.API.Contracts.Product;
using IS.ScaleModelsShop.API.Contracts.Product.CreateProduct;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetAllProductsPaginated;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductModel>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IMapper mapper, IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    public async Task<CreateProductModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var product = _mapper.Map<Product>(request);
        product.ProductCategory = new List<ProductCategory>();

        var productCategory = new ProductCategory();
        productCategory.Category = await _categoryRepository.GetByIdAsync(request.CategoryId);
        productCategory.CreatedDate = DateTime.UtcNow;
        productCategory.LastModifiedDate = DateTime.UtcNow;
        product.ProductCategory.Add(productCategory);

        product = await _productRepository.AddAsync(product);

        var result = _mapper.Map<CreateProductModel>(product);

        return result;
    }
}