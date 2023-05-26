using AutoMapper;
using IS.ScaleModelsShop.API.Contracts.Manufacturer;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer;

public class CreateManufacturerCommandHandler : IRequestHandler<CreateManufacturerCommand, ManufacturerModel>
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IMapper _mapper;

    public CreateManufacturerCommandHandler(IMapper mapper, IManufacturerRepository manufacturerRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _manufacturerRepository =
            manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
    }

    public async Task<ManufacturerModel> Handle(CreateManufacturerCommand request,
        CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var manufacturer = new Manufacturer
        {
            Name = request.Name,
            Website = request.Website,
            Id = Guid.NewGuid()
        };
        manufacturer = await _manufacturerRepository.AddAsync(manufacturer);

        var createManufacturer = _mapper.Map<ManufacturerModel>(manufacturer);


        return createManufacturer;
    }
}