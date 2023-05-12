using FluentValidation;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Application.Resources;

namespace IS.ScaleModelsShop.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator: AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator(ICategoryRepository categoryRepository)
        {
            _ = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));

            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                .MaximumLength(50)
                .MustAsync(async (name,_) => !await categoryRepository.AnyAsync(c => c.Name == name))
                .WithMessage(_ => ValidationErrors.Category_Common_SameCategoryAlreadyExist);
        }
    }
}