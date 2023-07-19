using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Commands.DeleteProduct;

/// <summary>
/// Command to delete a Product.
/// </summary>
public class DeleteProductCommand : IRequest
{
    /// <summary>
    /// Gets or sets ProductId.
    /// </summary>
    public Guid ProductId { get; set; }
}