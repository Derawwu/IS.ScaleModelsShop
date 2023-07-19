using IS.ScaleModelsShop.API.Common;
using IS.ScaleModelsShop.API.Contracts.Category;
using IS.ScaleModelsShop.Application.Features.Categories.Commands.CreateCategory;
using IS.ScaleModelsShop.Application.Models.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IS.ScaleModelsShop.API.Controllers;

/// <summary>
/// Controller for access to categories.
/// </summary>
[Route("[controller]")]
[ApiController]
public class CategoriesController : Controller
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoriesController"/> class.
    /// </summary>
    /// <param name="mediator">Instance of the <see cref="IMediator"/>.</param>
    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Gets categories extracted from database.
    /// </summary>
    /// <returns>Extracted categories.</returns>
    [HttpGet(Name = "GetAllCategories")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryModel>))]
    public async Task<IActionResult> GetAll()
    {
        var dtos = await _mediator.Send(new GetAllCategoriesQuery());

        return Ok(dtos);
    }

    /// <summary>
    /// Creates new category.
    /// </summary>
    /// <param name="createCategoryCommand">Create category request.</param>
    /// <returns>Created category model.</returns>
    [HttpPost(Name = "CreateCategory")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CategoryModel))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ExceptionResponse))]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand createCategoryCommand)
    {
        var createCategoryCommandResponse = await _mediator.Send(createCategoryCommand);

        return new OkObjectResult(createCategoryCommandResponse)
        {
            StatusCode = StatusCodes.Status201Created
        };
    }
}