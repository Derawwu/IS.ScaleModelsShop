using System.Runtime.Serialization;
using IS.ScaleModelsShop.API.Contracts.Product;
using IS.ScaleModelsShop.Application.Models.Queries;
using IS.ScaleModelsShop.Domain.Entities;

namespace IS.ScaleModelsShop.Application.Models.EDMs;

/// <summary>
/// Product entity domain model for using in the OData filter.
/// </summary>
[DataContract]
public class ProductEdm : QueryBase<IEnumerable<ProductModel>>
{
    /// <summary>
    /// Gets or sets <see cref="Product.Id"/>.
    /// </summary>
    [DataMember(Name = "id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets <see cref="Product.Name"/>.
    /// </summary>
    [DataMember(Name = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets <see cref="Product.Description"/>.
    /// </summary>
    [DataMember(Name = "description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets <see cref="Product.Price"/>.
    /// </summary>
    [DataMember(Name = "price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets <see cref="Product.ManufacturerId"/>.
    /// </summary>
    [DataMember(Name = "manufacturerId")]
    public Guid ManufacturerId { get; set; }

    /// <summary>
    /// Gets or sets <see cref="Product.CategoryId"/>.
    /// </summary>
    [DataMember(Name = "categoryId")]
    public Guid CategoryId { get; set; }
}