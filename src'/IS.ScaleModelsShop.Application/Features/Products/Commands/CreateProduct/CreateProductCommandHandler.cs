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
        private readonly IManufacturerRepository _manufacturerRepository;

        public CreateProductCommandHandler(IMapper mapper, IProductRepository productRepository, ICategoryRepository categoryRepository, IManufacturerRepository manufacturerRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
        }

        public async Task<GetProductDTO> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var validator =
                new CreateProductCommandValidator(_productRepository, _categoryRepository, _manufacturerRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

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