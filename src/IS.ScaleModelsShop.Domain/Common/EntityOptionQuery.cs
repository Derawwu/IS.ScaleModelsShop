namespace IS.ScaleModelsShop.Domain.Common;

/// <summary>
///     Data query options.
/// </summary>
/// <typeparam name="TEntity">
///     Generic entity type.
/// </typeparam>
public class EntityOptionQuery<TEntity>
{
    /// <summary>
    ///     Gets take entity query.
    /// </summary>
    public Func<IQueryable<TEntity>, IQueryable<TEntity>> TakeEntitiesQuery { get; }

    /// <summary>
    ///     Gets total count query.
    /// </summary>
    public Func<IQueryable<TEntity>, IQueryable<TEntity>> TotalCountQuery { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="EntityOptionQuery{TEntity}"/> class.
    /// </summary>
    /// <param name="takeEntitiesQuery">
    ///     Take entity query.
    /// </param>
    /// <param name="totalCountQuery">
    ///     Total count query.
    /// </param>
    /// <param name="accessPredicate">
    ///     The access predicate.
    /// </param>
    public EntityOptionQuery(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> takeEntitiesQuery,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> totalCountQuery)
    {
        this.TakeEntitiesQuery = takeEntitiesQuery;
        this.TotalCountQuery = totalCountQuery;
    }
}