using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest
{
    public Guid Id { get; set; } = Guid.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; } = default!;

    public Guid ManufacturerId { get; set; } = Guid.Empty;

    public Guid CategoryId { get; set; } = Guid.Empty;
}