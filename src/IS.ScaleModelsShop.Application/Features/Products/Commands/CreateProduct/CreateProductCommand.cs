using IS.ScaleModelsShop.API.Contracts.Product;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Create a Product command.
/// </summary>
public class CreateProductCommand : IRequest<ProductModel>
{
    /// <summary>
    /// Gets or sets Name of the Product to create.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets Description of the Product to create.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets Price of the Product to create.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets ManufacturerId of the Product to create.
    /// </summary>
    public Guid ManufacturerId { get; set; }

    /// <summary>
    /// Gets or sets CategoryId of the Product to create.
    /// </summary>
    public Guid CategoryId { get; set; }
}