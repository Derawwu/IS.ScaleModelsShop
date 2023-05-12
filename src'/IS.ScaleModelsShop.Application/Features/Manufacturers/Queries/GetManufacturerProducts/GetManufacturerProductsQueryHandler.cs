using AutoMapper;
using IS.ScaleModelsShop.Application.Exceptions;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Queries.GetManufacturerProducts
{
    public class GetManufacturerProductsQueryHandler : IRequestHandler<GetManufacturerProductsQuery, ManufacturerProductsViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IManufacturerRepository _manufacturerRepository;


        public GetManufacturerProductsQueryHandler(IMapper mapper, IManufacturerRepository manufacturerRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _manufacturerRepository =
                manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
        }

        public async Task<ManufacturerProductsViewModel> Handle(GetManufacturerProductsQuery request, CancellationToken cancellationToken)
        {
            if (!await _manufacturerRepository.AnyAsync(c => c.Name == request.ManufacturerName))
            {
                throw new NotFoundException(nameof(Manufacturer), request.ManufacturerName);
            }

            var entity = await _manufacturerRepository.GetManufacturerProductsAsync(request.ManufacturerName);

            return _mapper.Map<ManufacturerProductsViewModel>(entity);
        }
    }
}