using IS.ScaleModelsShop.Application.Exceptions;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var entityToDelete = await _productRepository.GetByIdAsync(request.ProductId);

            if (entityToDelete == null)
            {
                throw new NotFoundException(nameof(Product), request.ProductId);
            }

            await _productRepository.DeleteAsync(request.ProductId);
        }
    }
}