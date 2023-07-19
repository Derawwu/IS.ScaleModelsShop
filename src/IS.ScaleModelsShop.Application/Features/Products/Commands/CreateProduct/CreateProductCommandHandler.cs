using AutoMapper;
using IS.ScaleModelsShop.API.Contracts.Product;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Create a Product command handler.
/// </summary>
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductModel>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="CreateProductCommandHandler"/>
    /// </summary>
    /// <param name="mapper">Mapper instance.</param>
    /// <param name="productRepository">Instance for working with products.</param>
    /// <param name="categoryRepository">Instance for working with categories.</param>
    public CreateProductCommandHandler(IMapper mapper, IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <summary>
    /// Creates a new Product.
    /// </summary>
    /// <param name="request">Create Product command.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<ProductModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
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

        var result = _mapper.Map<ProductModel>(product);

        return result;
    }
}