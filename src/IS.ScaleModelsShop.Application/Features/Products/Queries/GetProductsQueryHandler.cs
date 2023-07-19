using AutoMapper;
using IS.ScaleModelsShop.API.Contracts.Product;
using IS.ScaleModelsShop.Application.Models.Queries;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Common;
using IS.ScaleModelsShop.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.OData.Query;

namespace IS.ScaleModelsShop.Application.Features.Products.Queries;

/// <summary>
/// Get products based on OData syntax query.
/// </summary>
public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PaginatedCollection<ProductModel>>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductsQueryHandler"/>
    /// </summary>
    /// <param name="mapper">Mapper instance.</param>
    /// <param name="productRepository">Instance for working with products.</param>
    public GetProductsQueryHandler(IMapper mapper, IProductRepository productRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    /// <summary>
    /// Gets products based on OData syntax.
    /// </summary>
    /// <param name="query">Get Products query.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<PaginatedCollection<ProductModel>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        _ = query ?? throw new ArgumentNullException(nameof(query));

        var entityOptionQuery = new EntityOptionQuery<Product>(
            q => (IQueryable<Product>)query.ODataQueryOptions.ApplyTo(q, AllowedQueryOptions.None),
            q => (IQueryable<Product>)query.ODataQueryOptions.ApplyTo(q, AllowedQueryOptions.Top | AllowedQueryOptions.Skip));

        var products = await _productRepository.GetByQueryAsync(
            entityOptionQuery,
            cancellationToken,
            asNoTracking: true);

        var productsPaginatedCollection =
            new PaginatedCollection<ProductModel>(_mapper.Map<IEnumerable<ProductModel>>(products.Items),
                products.TotalCount);

        return productsPaginatedCollection;
    }
}