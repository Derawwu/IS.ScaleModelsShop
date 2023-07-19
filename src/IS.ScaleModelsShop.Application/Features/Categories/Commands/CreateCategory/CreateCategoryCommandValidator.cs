using System.Net;
using FluentValidation;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Application.Resources;

namespace IS.ScaleModelsShop.Application.Features.Categories.Commands.CreateCategory;

/// <summary>
/// Validator for the Create Category command.
/// </summary>
public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="CreateCategoryCommandValidator"/>
    /// </summary>
    /// <param name="categoryRepository">Instance for working with Categories.</param>
    public CreateCategoryCommandValidator(ICategoryRepository categoryRepository)
    {
        _ = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(p => p.Name)
            .NotEmpty()
            .NotNull()
            .MaximumLength(50)
            .MustAsync(async (name, _) => !await categoryRepository.AnyAsync(c => c.Name == name))
            .WithMessage(_ => ValidationErrors.Category_Common_SameCategoryAlreadyExist);
    }
}