using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.UpdateManufacturer;

public class UpdateManufacturerCommand : IRequest
{
    public Guid Id { get; set; } = Guid.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Website { get; set; } = string.Empty;
}