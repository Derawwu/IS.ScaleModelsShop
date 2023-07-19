using IS.ScaleModelsShop.API.Contracts.Category;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Categories.Commands.CreateCategory;

/// <summary>
/// Create a category command.
/// </summary>
public class CreateCategoryCommand : IRequest<CategoryModel>
{
    /// <summary>
    /// Name of the Category to create.
    /// </summary>
    public string Name { get; set; }
}