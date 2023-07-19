using AutoMapper;
using IS.ScaleModelsShop.Application.Exceptions;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Commands.UpdateProduct;

/// <summary>
/// Update Product command handler.
/// </summary>
public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="UpdateProductCommandHandler"/>
    /// </summary>
    /// <param name="productRepository">Instance for working with products.</param>
    /// <param name="mapper">Mapper instance.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Updates a product.
    /// </summary>
    /// <param name="request">Update product command.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var entityToUpdate = await _productRepository.GetByIdAsync(request.Id);

        if (entityToUpdate == null) throw new EntityNotFoundException(nameof(Product), request.Id);

        _mapper.Map<UpdateProductCommand, Product>(request, entityToUpdate);

        await _productRepository.UpdateAsync(entityToUpdate);
    }
}