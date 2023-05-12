using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Categories.Queries.GetAllCategoriesList
{
    public class GetAllCategoriesListQuery : IRequest<List<CategoryListViewModel>>
    {
    }
}