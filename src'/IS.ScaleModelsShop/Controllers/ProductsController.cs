using AutoMapper;
using Azure;
using IS.ScaleModelsShop.Application.Features.Products.Commands.CreateProduct;
using IS.ScaleModelsShop.Application.Features.Products.Commands.DeleteProduct;
using IS.ScaleModelsShop.Application.Features.Products.Commands.UpdateProduct;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetAllProductsPaginated;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductByManufacturer;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductByName;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductsByCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IS.ScaleModelsShop.API.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController : Controller
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private const int MinimalPageSize = 1;
    private const int MinimalPageCount = 1;

    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet(Name = "GetProductsPaginated")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetProductsPaginated([FromQuery] int pageSize, int pageCount)
    {
        pageSize = pageSize < MinimalPageSize
            ? MinimalPageSize
            : pageSize;

        pageCount = pageCount < MinimalPageCount
            ? MinimalPageCount
            : pageCount;

        var query = new GetAllProductsPaginatedQuery()
        {
            PageSize = pageSize,
            PageNumber = pageCount
        };

        var response = await _mediator.Send(query);

        if (!response.Products.Any()) return new NoContentResult();

        return new OkObjectResult(response)
        {
            StatusCode = StatusCodes.Status200OK
        };
    }


    [HttpGet("{name}", Name = "GetProductByName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductByName([FromRoute] string name)
    {
        var request = new GetProductByNameQuery
        {
            Name = name
        };

        var dto = await _mediator.Send(request);

        return new OkObjectResult(dto)
        {
            StatusCode = StatusCodes.Status200OK
        };
    }

    [HttpGet("get-products-by-category/{categoryId}", Name = "GetAllProductsOfCategory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductsByCategory([FromRoute] Guid categoryId)
    {
        var query = new GetProductsByCategoryQuery
        {
            CategoryId = categoryId
        };

        var result = await _mediator.Send(query);

        if (!result.Any()) return new NoContentResult();

        return new OkObjectResult(result)
        {
            StatusCode = StatusCodes.Status200OK
        };
    }

    [HttpGet("get-products-by-manufacturer/{manufacturerId}", Name = "GetAllProductsOfManufacturer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductsByManufacturer([FromRoute] Guid manufacturerId)
    {
        var query = new GetProductsByManufacturerQuery
        {
            ManufacturerId = manufacturerId
        };

        var result = await _mediator.Send(query);

        if (!result.Any()) return new NoContentResult();

        return new OkObjectResult(result)
        {
            StatusCode = StatusCodes.Status200OK
        };
    }

    [HttpPost(Name = "CreateNewProduct")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        var dto = await _mediator.Send(command);

        return new OkObjectResult(dto)
        {
            StatusCode = StatusCodes.Status201Created
        };
    }

    [HttpPut("{productId}", Name = "UpdateProduct")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid productId, [FromBody] UpdateProductDTO body)
    {
        var command = _mapper.Map<UpdateProductCommand>(body);

        command.Id = productId;

        await _mediator.Send(command);

        return new NoContentResult();
    }

    [HttpDelete("{productId}", Name = "DeleteProduct")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid productId)
    {
        var command = new DeleteProductCommand { ProductId = productId };

        await _mediator.Send(command);

        return new NoContentResult();
    }
}