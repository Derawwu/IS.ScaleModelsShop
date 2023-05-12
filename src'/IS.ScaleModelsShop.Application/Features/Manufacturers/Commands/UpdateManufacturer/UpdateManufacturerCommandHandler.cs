using AutoMapper;
using FluentValidation;
using IS.ScaleModelsShop.Application.Exceptions;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.UpdateManufacturer
{
    public class UpdateManufacturerCommandHandler : IRequestHandler<UpdateManufacturerCommand>
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IMapper _mapper;

        public UpdateManufacturerCommandHandler(IManufacturerRepository manufacturerRepository, IMapper mapper)
        {
            _manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task Handle(UpdateManufacturerCommand request, CancellationToken cancellationToken)
        {
            var manufacturerToUpdate = await _manufacturerRepository.GetByIdAsync(request.Id);

            if (manufacturerToUpdate == null)
            {
                throw new NotFoundException(nameof(Manufacturer), request.Id);
            }

            _mapper.Map(request, manufacturerToUpdate, typeof(UpdateManufacturerCommand), typeof(Manufacturer));

            await _manufacturerRepository.UpdateAsync(manufacturerToUpdate);
        }
    }
}