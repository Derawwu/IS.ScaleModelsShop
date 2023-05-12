using FluentValidation;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Application.Resources;

namespace IS.ScaleModelsShop.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator(IProductRepository productRepository)
        {
            _ = productRepository ?? throw new ArgumentNullException(nameof(productRepository));

            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(p => p.ProductId)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (id, _) => await productRepository.AnyAsync(p => p.Id == id))
                .WithMessage(p => string.Format(ValidationErrors.Products_Common_ProductDoesNotExist, p.ProductId));
        }
    }
}