using IS.ScaleModelsShop.API.Common;
using IS.ScaleModelsShop.Application.Features.Categories.Commands.CreateCategory;
using IS.ScaleModelsShop.Application.Features.Categories.Queries.GetAllCategoriesList;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductsByCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IS.ScaleModelsShop.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("all", Name = "GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllCategoriesList()
        {
            var dtos = await _mediator.Send(new GetAllCategoriesListQuery());

            if (!dtos.Any())
            {
                return NoContent();
            }

            return Ok(dtos);
        }

        //[HttpGet("{categoryName}", Name = "GetAllProductsOfCategory")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetCategoryProducts(string categoryName)
        //{
        //    var query = new GetCategoryProductsQuery()
        //    {
        //        CategoryName = categoryName
        //    };

        //    var dtos = await _mediator.Send(query);

        //    if (dtos == null)
        //    {
        //        return NoContent();
        //    }

        //    return Ok(dtos);
        //}

        [HttpPost(Name = "CreateCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
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
}