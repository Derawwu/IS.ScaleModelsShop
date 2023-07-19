using System.Diagnostics.CodeAnalysis;

namespace IS.ScaleModelsShop.Domain.Common
{
    /// <summary>
    ///     The model represents the content of the specific page.
    /// </summary>
    /// <typeparam name="T">
    ///     Type of the entity.
    /// </typeparam>
    public class PaginatedCollection<T>
        where T : class
    {
        /// <summary>
        ///     Gets collection of entities.
        /// </summary>
        public IEnumerable<T> Items { get; }

        /// <summary>
        ///     Gets total count of entities in the storage.
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PaginatedCollection{T}"/> class.
        /// </summary>
        /// <param name="items">
        ///     Items collection.
        /// </param>
        /// <param name="totalCount">
        ///     Items total count.
        /// </param>
        public PaginatedCollection(IEnumerable<T> items, int totalCount)
        {
            this.Items = items;
            this.TotalCount = totalCount;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PaginatedCollection{T}"/> class.
        /// </summary>
        /// <param name="items">
        ///     Items collection.
        /// </param>
        public PaginatedCollection(IEnumerable<T> items)
        {
            this.Items = items;
        }
    }
}