using AutoMapper;
using IS.ScaleModelsShop.Application.Exceptions;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.UpdateManufacturer;

/// <summary>
/// Update a Manufacturer command handler.
/// </summary>
public class UpdateManufacturerCommandHandler : IRequestHandler<UpdateManufacturerCommand>
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of <see cref="UpdateManufacturerCommandHandler"/>
    /// </summary>
    /// <param name="manufacturerRepository">Instance for working with manufacturers.</param>
    /// <param name="mapper">Mapper instance.</param>
    public UpdateManufacturerCommandHandler(IManufacturerRepository manufacturerRepository, IMapper mapper)
    {
        _manufacturerRepository =
            manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Updates the Manufacturer.
    /// </summary>
    /// <param name="request">Update Manufacturer command.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task Handle(UpdateManufacturerCommand request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var manufacturerToUpdate = await _manufacturerRepository.GetByIdAsync(request.Id);

        if (manufacturerToUpdate is null) throw new EntityNotFoundException(nameof(Manufacturer), request.Id);

        _mapper.Map(request, manufacturerToUpdate, typeof(UpdateManufacturerCommand), typeof(Manufacturer));

        await _manufacturerRepository.UpdateAsync(manufacturerToUpdate);
    }
}