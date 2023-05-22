using AutoMapper;
using IS.ScaleModelsShop.Application.Exceptions;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entityToUpdate = await _productRepository.GetByIdAsync(request.Id);

        if (entityToUpdate == null) throw new NotFoundException(nameof(Product), request.Id);

        _mapper.Map(request, entityToUpdate, typeof(UpdateProductCommand), typeof(Product));

        await _productRepository.UpdateAsync(entityToUpdate);
    }
}