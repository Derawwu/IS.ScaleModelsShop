using AutoMapper;
using FluentValidation;
using IS.ScaleModelsShop.Application.Exceptions;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetAllProductsPaginated;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, GetProductDTO>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CreateProductCommandHandler(IMapper mapper, IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<GetProductDTO> Handle(CreateProductCommand request, CancellationToken cancellationToken)
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

            var result = _mapper.Map<GetProductDTO>(product);

            return result;
        }
    }
}