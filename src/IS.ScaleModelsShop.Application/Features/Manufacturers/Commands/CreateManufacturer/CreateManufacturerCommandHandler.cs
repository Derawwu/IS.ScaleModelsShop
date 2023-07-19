using AutoMapper;
using IS.ScaleModelsShop.API.Contracts.Manufacturer;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer;

/// <summary>
/// Create a Manufacturer command handler.
/// </summary>
public class CreateManufacturerCommandHandler : IRequestHandler<CreateManufacturerCommand, ManufacturerModel>
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of <see cref="CreateManufacturerCommandHandler"/>
    /// </summary>
    /// <param name="mapper">Mapper instance.</param>
    /// <param name="manufacturerRepository">Instance for working with manufacturers.</param>
    public CreateManufacturerCommandHandler(IMapper mapper, IManufacturerRepository manufacturerRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _manufacturerRepository =
            manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
    }

    /// <summary>
    /// Creates a new Manufacturer.
    /// </summary>
    /// <param name="request">Request to create a Manufacturer.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<ManufacturerModel> Handle(CreateManufacturerCommand request,
        CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var manufacturer = new Manufacturer
        {
            Name = request.Name,
            Website = request.Website
        };

        manufacturer = await _manufacturerRepository.AddAsync(manufacturer);

        var createManufacturer = _mapper.Map<ManufacturerModel>(manufacturer);

        return createManufacturer;
    }
}