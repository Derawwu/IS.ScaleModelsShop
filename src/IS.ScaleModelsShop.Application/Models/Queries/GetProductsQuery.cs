using IS.ScaleModelsShop.API.Contracts.Product;
using IS.ScaleModelsShop.Domain.Common;
using IS.ScaleModelsShop.Domain.Entities;
using Microsoft.AspNetCore.OData.Query;

namespace IS.ScaleModelsShop.Application.Models.Queries;

public class GetProductsQuery : QueryBase<PaginatedCollection<ProductModel>>
{
    /// <summary>
    /// Gets or sets <see cref="ODataQueryOptions{ProductModel}"/>.
    /// </summary>
    public ODataQueryOptions<Product> ODataQueryOptions { get; set; }
}