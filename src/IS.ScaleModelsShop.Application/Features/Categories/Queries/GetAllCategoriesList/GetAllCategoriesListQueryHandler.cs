using AutoMapper;
using IS.ScaleModelsShop.API.Contracts.Category;
using IS.ScaleModelsShop.Application.Models.Queries;
using IS.ScaleModelsShop.Application.Repositories;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Categories.Queries.GetAllCategoriesList;

/// <summary>
/// Get all Categories query handler.
/// </summary>
public class GetAllCategoriesListQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryModel>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of <see cref="GetAllCategoriesListQueryHandler"/>
    /// </summary>
    /// <param name="mapper">Mapper instance.</param>
    /// <param name="categoryRepository">Instance for working with categories.</param>
    public GetAllCategoriesListQueryHandler(IMapper mapper, ICategoryRepository categoryRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <summary>
    /// Gets all Categories.
    /// </summary>
    /// <param name="request">Incoming HTTP request to get all Categories.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<List<CategoryModel>> Handle(GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var allCategories = (await _categoryRepository.GetAllAsync()).OrderBy(x => x.Name);

        return _mapper.Map<List<CategoryModel>>(allCategories);
    }
}