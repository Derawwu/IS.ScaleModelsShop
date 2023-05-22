using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer;

public class CreateManufacturerCommand : IRequest<CreateManufacturerDTO>
{
    public string Name { get; set; } = string.Empty;
}