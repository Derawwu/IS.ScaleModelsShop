using AutoMapper;
using IS.ScaleModelsShop.API.Contracts.Manufacturer.UpdateManufacturer;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.UpdateManufacturer;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Queries.GetAllManufacturersList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IS.ScaleModelsShop.API.Controllers;

[Route("[controller]")]
[ApiController]
public class ManufacturerController : Controller
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ManufacturerController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet("all", Name = "GetAllManufacturers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllManufacturers()
    {
        var dtos = await _mediator.Send(new GetAllManufacturersListQuery());

        if (!dtos.Any()) return new NoContentResult();

        return new OkObjectResult(dtos)
        {
            StatusCode = StatusCodes.Status200OK
        };
    }

    [HttpPost("createManufacturer", Name = "CreateNewManufacturer")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateNewManufacturer([FromBody] CreateManufacturerCommand body)
    {
        var dto = await _mediator.Send(body);

        return new OkObjectResult(dto)
        {
            StatusCode = StatusCodes.Status201Created
        };
    }

    [HttpPut("{manufacturerId}", Name = "UpdateManufacturer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateManufacturer([FromRoute] Guid manufacturerId,
        [FromBody] UpdateManufacturerModel body)
    {
        var command = _mapper.Map<UpdateManufacturerCommand>(body);
        command.Id = manufacturerId;

        await _mediator.Send(command);

        return NoContent();
    }
}