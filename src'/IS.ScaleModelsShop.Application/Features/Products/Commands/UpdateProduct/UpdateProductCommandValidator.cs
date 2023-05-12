using FluentValidation;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Application.Resources;

namespace IS.ScaleModelsShop.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator(IProductRepository productRepository)
        {
            _ = productRepository ?? throw new ArgumentNullException(nameof(productRepository));

            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(p => p.Id)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (id, _) => await productRepository.AnyAsync(p => p.Id == id))
                .WithMessage(p => string.Format(ValidationErrors.Products_Common_ProductDoesNotExist, p.Id));

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
                .MustAsync(async (id, _) => !await productRepository.AnyAsync(c => c.Id == id))
                .WithMessage(p => string.Format(ValidationErrors.Product_Manufacturer_ManufacturerDoesNotExist, p.ManufacturerId));

            RuleFor(p => p.CategoryId)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (id, _) => !await productRepository.AnyAsync(c => c.Id == id))
                .WithMessage(p => string.Format(ValidationErrors.Product_Categoty_CategotyDoesNotExist, p.CategoryId));
        }
    }
}