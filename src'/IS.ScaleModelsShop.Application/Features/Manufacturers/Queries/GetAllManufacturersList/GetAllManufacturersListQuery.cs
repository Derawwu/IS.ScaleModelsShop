using IS.ScaleModelsShop.API.Contracts.Manufacturer;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Queries.GetAllManufacturersList;

public class GetAllManufacturersListQuery : IRequest<List<ManufacturerModel>>
{
}