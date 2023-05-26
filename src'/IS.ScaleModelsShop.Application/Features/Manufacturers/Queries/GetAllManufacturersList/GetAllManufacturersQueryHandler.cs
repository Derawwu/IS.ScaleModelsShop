﻿using AutoMapper;
using IS.ScaleModelsShop.API.Contracts.Manufacturer;
using IS.ScaleModelsShop.Application.Repositories;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Queries.GetAllManufacturersList;

public class
    GetAllManufacturersQueryHandler : IRequestHandler<GetAllManufacturersListQuery, List<ManufacturerModel>>
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IMapper _mapper;

    public GetAllManufacturersQueryHandler(IMapper mapper, IManufacturerRepository manufacturerRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _manufacturerRepository =
            manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
    }

    public async Task<List<ManufacturerModel>> Handle(GetAllManufacturersListQuery request,
        CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var list = (await _manufacturerRepository.GetAllAsync()).OrderBy(m => m.Name);

        return _mapper.Map<List<ManufacturerModel>>(list);
    }
}