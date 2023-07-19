using AutoMapper;
using IS.ScaleModelsShop.API.Contracts.Category;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Categories.Commands.CreateCategory;

/// <summary>
/// Create a category command handler.
/// </summary>
public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryModel>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of <see cref="CreateCategoryCommandHandler"/>
    /// </summary>
    /// <param name="categoryRepository">Instance for working with categories.</param>
    /// <param name="mapper">Mapper instance.</param>
    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Create a Category.
    /// </summary>
    /// <param name="request">A Category create command.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<CategoryModel> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var category = new Category
        {
            Name = request.Name,
            Id = Guid.NewGuid()
        };

        category = await _categoryRepository.AddAsync(category);

        var createdCategory = _mapper.Map<CategoryModel>(category);

        return createdCategory;
    }
}