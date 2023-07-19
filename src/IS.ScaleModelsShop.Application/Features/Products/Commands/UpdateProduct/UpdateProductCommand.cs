using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Commands.UpdateProduct;

/// <summary>
/// Command to update a Product.
/// </summary>
public class UpdateProductCommand : IRequest
{
    /// <summary>
    /// Gets or sets Id of the Product.
    /// </summary>
    public Guid Id { get; set; } = Guid.Empty;

    /// <summary>
    /// Gets or sets Name of the Product.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets Description of the Product.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets Price of the Product.
    /// </summary>
    public decimal Price { get; set; } = default!;

    /// <summary>
    /// Gets or sets ManufacturerId of the Product.
    /// </summary>
    public Guid ManufacturerId { get; set; } = Guid.Empty;

    /// <summary>
    /// Gets or sets CategoryId of the Product.
    /// </summary>
    public Guid CategoryId { get; set; } = Guid.Empty;
}