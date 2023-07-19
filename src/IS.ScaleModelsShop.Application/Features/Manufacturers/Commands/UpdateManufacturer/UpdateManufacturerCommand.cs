using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.UpdateManufacturer;

/// <summary>
/// Command to update a Manufacturer.
/// </summary>
public class UpdateManufacturerCommand : IRequest
{
    /// <summary>
    /// Guid of the Manufacturer to update.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the Manufacturer to update.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Website of the Manufacturer to update.
    /// </summary>
    public string? Website { get; set; }
}