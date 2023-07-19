namespace IS.ScaleModelsShop.API.Contracts.Product
{
    /// <summary>
    /// Base contract for Product.
    /// </summary>
    public class ProductModel
    {
        /// <summary>
        /// Gets or sets Guid of the Product.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets Name of the Product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Description of the Product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Price of the Product.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets ManufacturerId of the Product.
        /// </summary>
        public Guid ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets CategoryId of the Product.
        /// </summary>
        public Guid CategoryId { get; set; }
    }
}
