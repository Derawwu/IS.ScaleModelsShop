using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<CreateCategoryDTO>
{
    public string Name { get; set; } = string.Empty;
}