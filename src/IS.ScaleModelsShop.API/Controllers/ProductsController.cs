using AutoMapper;
using IS.ScaleModelsShop.API.Attributes;
using IS.ScaleModelsShop.API.Constants;
using IS.ScaleModelsShop.API.Contracts.Product;
using IS.ScaleModelsShop.API.Contracts.Product.UpdateProduct;
using IS.ScaleModelsShop.Application.Features.Products.Commands.CreateProduct;
using IS.ScaleModelsShop.Application.Features.Products.Commands.DeleteProduct;
using IS.ScaleModelsShop.Application.Features.Products.Commands.UpdateProduct;
using IS.ScaleModelsShop.Application.Models.Queries;
using IS.ScaleModelsShop.Domain.Common;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace IS.ScaleModelsShop.API.Controllers;

/// <summary>
/// Controller for access to products.
/// </summary>
[Route("[controller]")]
[ApiController]
public class ProductsController : Controller
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes new instance of <see cref="ProductsController"/>.>
    /// </summary>
    /// <param name="mediator">Instance of the <see cref="IMediator"/>.</param>
    /// <param name="mapper">Instance of the <see cref="IMapper"/>.</param>
    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Gets Products extracted based on OData filter.
    /// </summary>
    /// <param name="oDataQueryOptions"><see cref="ODataQueryOptions{LocationPersonnel}"/>.</param>
    /// <returns>Products extracted from the database.</returns>
    [HttpGet]
    [SwaggerODataParameterSetup(Name = SwaggerODataConstants.Filter, IsRequired = false, Example = "manufacturerId eq 14BCD768-3504-46EF-968F-8580F38CD675")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductModel>))]
    public async Task<IActionResult> GetProducts(ODataQueryOptions<Product> oDataQueryOptions)
    {
        var products = await _mediator.Send(new GetProductsQuery { ODataQueryOptions = oDataQueryOptions });
        Response.Headers.Add(Headers.TotalCountHeader, products.TotalCount.ToString());

        return Ok(products.Items);
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="createProductRequest">Create product request.</param>
    /// <returns>Created product model.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand createProductRequest)
    {
        var dto = await _mediator.Send(createProductRequest);

        return new OkObjectResult(dto)
        {
            StatusCode = StatusCodes.Status201Created
        };
    }

    /// <summary>
    /// Updates the product.
    /// </summary>
    /// <param name="productId">Guid of the product to update.</param>
    /// <param name="updateProductRequest">Update product request.</param>
    /// <returns>A <see cref="NoContentResult"/> result.</returns>
    [HttpPut("{productId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid productId, [FromBody] UpdateProductModel updateProductRequest)
    {
        var command = _mapper.Map<UpdateProductCommand>(updateProductRequest);

        command.Id = productId;

        await _mediator.Send(command);

        return NoContent();
    }

    /// <summary>
    /// Deletes the product from the database.
    /// </summary>
    /// <param name="productId">Guid of the product to delete.</param>
    /// <returns>A <see cref="NoContentResult"/> result.</returns>
    [HttpDelete("{productId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid productId)
    {
        var command = new DeleteProductCommand { ProductId = productId };

        await _mediator.Send(command);

        return NoContent();
    }
}