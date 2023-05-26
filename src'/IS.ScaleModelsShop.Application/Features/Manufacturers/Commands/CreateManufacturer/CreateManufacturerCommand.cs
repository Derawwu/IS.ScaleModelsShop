using IS.ScaleModelsShop.API.Contracts.Manufacturer;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer;

public class CreateManufacturerCommand : IRequest<ManufacturerModel>
{
    public string Name { get; set; } = string.Empty;

    public string Website { get; set; } = string.Empty;
}