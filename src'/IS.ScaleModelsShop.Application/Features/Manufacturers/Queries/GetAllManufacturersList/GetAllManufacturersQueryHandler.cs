using AutoMapper;
using IS.ScaleModelsShop.Application.Repositories;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Queries.GetAllManufacturersList
{
    public class GetAllManufacturersQueryHandler : IRequestHandler<GetAllManufacturersListQuery, List<ManufacturersListViewModel>>
    {
        private readonly IMapper _mapper;

        private readonly IManufacturerRepository _manufacturerRepository;

        public GetAllManufacturersQueryHandler(IMapper mapper, IManufacturerRepository manufacturerRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
        }

        public async Task<List<ManufacturersListViewModel>> Handle(GetAllManufacturersListQuery request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            var list = (await _manufacturerRepository.GetAllAsync()).OrderBy(m => m.Name);

            return _mapper.Map<List<ManufacturersListViewModel>>(list);
        }
    }
}