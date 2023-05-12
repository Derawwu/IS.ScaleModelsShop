﻿using AutoMapper;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.UpdateManufacturer;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Queries.GetAllManufacturersList;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Queries.GetManufacturerProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IS.ScaleModelsShop.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ManufacturerController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

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

            if (!dtos.Any())
            {
                return NoContent();
            }

            return Ok(dtos);
        }

        [HttpGet("{manufacturerName}", Name = "GetAllProductsOfManufacturer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetManufacturerProducts(string manufacturerName)
        {
            var query = new GetManufacturerProductsQuery()
            {
                ManufacturerName = manufacturerName
            };

            var dto = await _mediator.Send(query);

            if (dto == null)
            {
                return NoContent();
            }

            return Ok(dto);
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
        public async Task<IActionResult> UpdateManufacturer([FromRoute] Guid manufacturerId,[FromBody] UpdateManufacturerDTO body)
        {
            var command = _mapper.Map<UpdateManufacturerCommand>(body);
            command.Id = manufacturerId;

            await _mediator.Send(command);

            return NoContent();
        }
    }
}