using AutoMapper;
using IS.ScaleModelsShop.API.Contracts.Manufacturer;
using IS.ScaleModelsShop.API.Contracts.Manufacturer.UpdateManufacturer;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.UpdateManufacturer;
using IS.ScaleModelsShop.Application.Models.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IS.ScaleModelsShop.API.Controllers;

/// <summary>
/// Controller for access to manufacturers.
/// </summary>
[Route("[controller]")]
[ApiController]
public class ManufacturersController : Controller
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ManufacturersController"/> class.
    /// </summary>
    /// <param name="mediator">Instance of the <see cref="IMediator"/>.</param>
    /// <param name="mapper">Instance of the <see cref="IMapper"/>.</param>
    public ManufacturersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Gets manufacturers extracted from database.
    /// </summary>
    /// <returns>Extracted manufacturers.</returns>
    [HttpGet(Name = "GetAllManufacturers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ManufacturerModel>))]
    public async Task<IActionResult> GetAllManufacturers()
    {
        var dtos = await _mediator.Send(new GetAllManufacturersQuery());

        return Ok(dtos);
    }

    /// <summary>
    /// Creates new manufacturer.
    /// </summary>
    /// <param name="createManufacturerCommand">Create manufacturer command.</param>
    /// <returns>Created manufacturer model.</returns>
    [HttpPost(Name = "CreateNewManufacturer")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ManufacturerModel))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateNewManufacturer([FromBody] CreateManufacturerCommand createManufacturerCommand)
    {
        var dto = await _mediator.Send(createManufacturerCommand);

        return new OkObjectResult(dto)
        {
            StatusCode = StatusCodes.Status201Created
        };
    }

    /// <summary>
    /// Updates a manufacturer.
    /// </summary>
    /// <param name="manufacturerId">Guid of the manufacturer to update.</param>
    /// <param name="updateManufacturerCommand">Update manufacturer request.</param>
    /// <returns>A <see cref="NoContentResult"/> result.</returns>
    [HttpPut("{manufacturerId}", Name = "UpdateManufacturer")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContentResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateManufacturer([FromRoute] Guid manufacturerId,
        [FromBody] UpdateManufacturerModel updateManufacturerCommand)
    {
        var command = _mapper.Map<UpdateManufacturerCommand>(updateManufacturerCommand);
        command.Id = manufacturerId;

        await _mediator.Send(command);

        return NoContent();
    }
}