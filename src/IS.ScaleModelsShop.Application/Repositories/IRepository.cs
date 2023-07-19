using IS.ScaleModelsShop.Domain.Common;
using System.Linq.Expressions;

namespace IS.ScaleModelsShop.Application.Repositories;

/// <summary>
/// Base repository for working with context.
/// </summary>
/// <typeparam name="TEntity">Entity type.</typeparam>
public interface IRepository<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Gets all entities from a database.
    /// </summary>
    /// <returns>All entities from a database.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Gets an entity by Guid.
    /// </summary>
    /// <param name="id">Guid of the entity.</param>
    /// <param name="asNoTracking">Defines if entity should be fetched as no tracking.</param>
    /// <returns>Entity with matching Guid.</returns>
    Task<TEntity?> GetByIdAsync(Guid id, bool asNoTracking = false);

    /// <summary>
    /// Adds entity to a database.
    /// </summary>
    /// <param name="entity">An entity which should be added.</param>
    /// <returns>Added entity.</returns>
    Task<TEntity> AddAsync(TEntity entity);

    /// <summary>
    /// Updates an entity in a database.
    /// </summary>
    /// <param name="entity">An entity which should be updated.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Removes an entity from the database.
    /// </summary>
    /// <param name="id">Guid of the entity.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Asynchronously determines whether any element of a sequence satisfies a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity> GetEntityByNameAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Gets entities by selector and predicate from a database.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="selector">A projection function to apply to each element.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <param name="asNoTracking">Indicates whether to track the results of a query or not.
    /// By default is true.</param>
    /// <typeparam name="TResult">The type of the value returned by the function represented by selector.</typeparam>
    /// <returns>Filtered entities from a database.</returns>
    Task<IEnumerable<TResult>> FilterAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default,
        bool asNoTracking = true);

    /// <summary>
    /// Gets entities by query parameters from a database.
    /// </summary>
    /// <param name="entityOptionQuery"><see><cref>DataQueryOptions</cref></see>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <param name="asNoTracking">Indicates whether to track the results of a query or not. By default is true.</param>
    /// <returns>All entities by query parameters from a database.</returns>
    Task<PaginatedCollection<TEntity>> GetByQueryAsync(
        EntityOptionQuery<TEntity> entityOptionQuery,
        CancellationToken cancellationToken = default,
        bool asNoTracking = true);

    /// <summary>
    /// Removes all entities in the database.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RemoveAllAsync();
}