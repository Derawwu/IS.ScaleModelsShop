using IS.ScaleModelsShop.API.Contracts.Product.CreateProduct;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<CreateProductModel>
{
    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public Guid ManufacturerId { get; set; }
    public Guid CategoryId { get; set; }
}