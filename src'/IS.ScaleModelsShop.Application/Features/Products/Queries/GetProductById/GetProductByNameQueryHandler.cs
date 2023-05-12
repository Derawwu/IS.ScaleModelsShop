using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IS.ScaleModelsShop.Application.Exceptions;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;

namespace IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, GetProductByNameViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public GetProductByNameQueryHandler(IMapper mapper, IMediator mediator, IProductRepository productRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<GetProductByNameViewModel> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetEntityByNameAsync(p => p.Name == request.Name);

            if (product == null)
            {
                throw new NotFoundException(nameof(Product), request.Name);
            }

            var productToReturn = _mapper.Map<GetProductByNameViewModel>(product);

            return productToReturn;
        }
    }
}
