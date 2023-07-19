using IS.ScaleModelsShop.API.Contracts.Manufacturer;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer;

/// <summary>
/// Command to create a Manufacturer.
/// </summary>
public class CreateManufacturerCommand : IRequest<ManufacturerModel>
{
    /// <summary>
    /// Name of the Manufacturer to create.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Website of the Manufacturer to create.
    /// </summary>
    public string? Website { get; set; }
}