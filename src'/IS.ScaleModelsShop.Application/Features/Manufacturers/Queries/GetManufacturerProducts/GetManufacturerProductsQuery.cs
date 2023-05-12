using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Queries.GetManufacturerProducts
{
    public class GetManufacturerProductsQuery : IRequest<ManufacturerProductsViewModel>
    {
        public string ManufacturerName { get; set; } = string.Empty;
    }
}