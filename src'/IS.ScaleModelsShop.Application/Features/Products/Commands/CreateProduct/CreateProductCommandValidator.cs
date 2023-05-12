﻿using FluentValidation;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Application.Resources;

namespace IS.ScaleModelsShop.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator(IProductRepository productRepository, ICategoryRepository categoryRepository, IManufacturerRepository manufacturerRepository)
        {
            _ = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _ = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _ = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));

            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                .MaximumLength(50)
                .MustAsync(async (name, _) => !await productRepository.AnyAsync(c => c.Name == name))
                .WithMessage(_ => ValidationErrors.Product_Common_SameProductAlreadyExist);

            RuleFor(p => p.Description)
                .MaximumLength(500);

            RuleFor(p => p.Price)
                .NotEmpty()
                .NotNull();

            RuleFor(p => p.ManufacturerId)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (id, _) => await manufacturerRepository.AnyAsync(c => c.Id == id))
                .WithMessage(p => string.Format(ValidationErrors.Product_Manufacturer_ManufacturerDoesNotExist, p.ManufacturerId));

            RuleFor(p => p.CategoryId)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (id, _) => await categoryRepository.AnyAsync(c => c.Id == id))
                .WithMessage(p => string.Format(ValidationErrors.Product_Categoty_CategotyDoesNotExist, p.CategoryId));
        }
    }
}