using AutoMapper;
using IS.ScaleModelsShop.API.Contracts.Category;
using IS.ScaleModelsShop.Application.Repositories;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Categories.Queries.GetAllCategoriesList;

public class GetAllCategoriesListQueryHandler : IRequestHandler<GetAllCategoriesListQuery, List<CategoryModel>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetAllCategoriesListQueryHandler(IMapper mapper, ICategoryRepository categoryRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    public async Task<List<CategoryModel>> Handle(GetAllCategoriesListQuery request,
        CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var allCategories = (await _categoryRepository.GetAllAsync()).OrderBy(x => x.Name);

        return _mapper.Map<List<CategoryModel>>(allCategories);
    }
}