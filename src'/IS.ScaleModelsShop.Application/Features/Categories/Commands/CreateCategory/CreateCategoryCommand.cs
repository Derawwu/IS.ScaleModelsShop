using IS.ScaleModelsShop.API.Contracts.Category;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<CategoryModel>
{
    public string Name { get; set; } = string.Empty;
}