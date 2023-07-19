using AutoMapper;
using IS.ScaleModelsShop.API.Contracts.Manufacturer;
using IS.ScaleModelsShop.Application.Models.Queries;
using IS.ScaleModelsShop.Application.Repositories;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Queries.GetAllManufacturersList;

/// <summary>
/// Get all Manufacturers query handler.
/// </summary>
public class GetAllManufacturersQueryHandler : IRequestHandler<GetAllManufacturersQuery, List<ManufacturerModel>>
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of <see cref="GetAllManufacturersQueryHandler"/>
    /// </summary>
    /// <param name="mapper">Mapper instance.</param>
    /// <param name="manufacturerRepository">Instance for working with manufacturers.</param>
    public GetAllManufacturersQueryHandler(IMapper mapper, IManufacturerRepository manufacturerRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _manufacturerRepository =
            manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
    }

    /// <summary>
    /// Gets all Manufacturers.
    /// </summary>
    /// <param name="request">Request to get all Manufacturers.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<List<ManufacturerModel>> Handle(GetAllManufacturersQuery request,
        CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var list = (await _manufacturerRepository.GetAllAsync()).OrderBy(m => m.Name);

        return _mapper.Map<List<ManufacturerModel>>(list);
    }
}