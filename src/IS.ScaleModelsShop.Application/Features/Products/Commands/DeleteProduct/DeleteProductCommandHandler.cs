using IS.ScaleModelsShop.Application.Exceptions;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Commands.DeleteProduct;

/// <summary>
/// Delete Product command handler.
/// </summary>
public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="DeleteProductCommandHandler"/>
    /// </summary>
    /// <param name="productRepository">Instance for working with products.</param>
    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    /// <summary>
    /// Deletes a product.
    /// </summary>
    /// <param name="request">Delete Product command.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var entityToDelete = await _productRepository.GetByIdAsync(request.ProductId);

        if (entityToDelete == null) throw new EntityNotFoundException(nameof(Product), request.ProductId);

        await _productRepository.DeleteAsync(request.ProductId);
    }
}