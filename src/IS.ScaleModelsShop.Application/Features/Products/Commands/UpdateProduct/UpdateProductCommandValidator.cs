using FluentValidation;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Application.Resources;

namespace IS.ScaleModelsShop.Application.Features.Products.Commands.UpdateProduct;

/// <summary>
/// Validator for the create Product command.
/// </summary>
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProductCommandValidator"/>
    /// </summary>
    /// <param name="productRepository">Instance for working with products.</param>
    /// <param name="categoryRepository">Instance for working with categories.</param>
    /// <param name="manufacturerRepository">Instance for working with manufacturers.</param>
    public UpdateProductCommandValidator(IProductRepository productRepository, ICategoryRepository categoryRepository, IManufacturerRepository manufacturerRepository)
    {
        _ = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _ = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _ = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(p => p.Id)
            .Must(id => id != Guid.Empty)
            .WithMessage(_ => ValidationErrors.Product_Common_GuidCanNotBeEmptyGuid)
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
            .GreaterThan(decimal.Zero)
            .WithMessage(p => string.Format(ValidationErrors.Product_Common_PriceShouldBeGreaterThanZero, p.Price));

        RuleFor(p => p.ManufacturerId)
            .Must(id => id != Guid.Empty)
            .WithName(x => nameof(x.ManufacturerId))
            .WithMessage(_ => ValidationErrors.Product_Common_GuidCanNotBeEmptyGuid)
            .MustAsync(async (id, _) => await manufacturerRepository.AnyAsync(c => c.Id == id))
            .WithMessage(p =>
                string.Format(ValidationErrors.Product_Manufacturer_ManufacturerDoesNotExist, p.ManufacturerId));

        RuleFor(p => p.CategoryId)
            .Must(id => id != Guid.Empty)
            .WithName(x => nameof(x.CategoryId))
            .WithMessage(_ => ValidationErrors.Product_Common_GuidCanNotBeEmptyGuid)
            .MustAsync(async (id, _) => await categoryRepository.AnyAsync(c => c.Id == id))
            .WithMessage(p => string.Format(ValidationErrors.Product_Categoty_CategotyDoesNotExist, p.CategoryId));
    }
}