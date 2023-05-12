using AutoMapper;
using FluentValidation;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer
{
    public class CreateManufacturerCommandHandler : IRequestHandler<CreateManufacturerCommand, CreateManufacturerDTO>
    {
        private readonly IMapper _mapper;

        private readonly IManufacturerRepository _manufacturerRepository;

        public CreateManufacturerCommandHandler(IMapper mapper, IManufacturerRepository manufacturerRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _manufacturerRepository =
                manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
        }

        public async Task<CreateManufacturerDTO> Handle(CreateManufacturerCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateManufacturerCommandValidator(_manufacturerRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var manufacturer = new Manufacturer()
            {
                Name = request.Name,
                Id = Guid.NewGuid()
            };
            manufacturer = await _manufacturerRepository.AddAsync(manufacturer);

            var createManufacturer = _mapper.Map<CreateManufacturerDTO>(manufacturer);


            return createManufacturer;
        }
    }
}