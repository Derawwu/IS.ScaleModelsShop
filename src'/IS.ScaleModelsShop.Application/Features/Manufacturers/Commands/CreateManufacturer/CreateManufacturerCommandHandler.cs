using AutoMapper;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer;

public class CreateManufacturerCommandHandler : IRequestHandler<CreateManufacturerCommand, CreateManufacturerDTO>
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IMapper _mapper;

    public CreateManufacturerCommandHandler(IMapper mapper, IManufacturerRepository manufacturerRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _manufacturerRepository =
            manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
    }

    public async Task<CreateManufacturerDTO> Handle(CreateManufacturerCommand request,
        CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var manufacturer = new Manufacturer
        {
            Name = request.Name,
            Id = Guid.NewGuid()
        };
        manufacturer = await _manufacturerRepository.AddAsync(manufacturer);

        var createManufacturer = _mapper.Map<CreateManufacturerDTO>(manufacturer);


        return createManufacturer;
    }
}