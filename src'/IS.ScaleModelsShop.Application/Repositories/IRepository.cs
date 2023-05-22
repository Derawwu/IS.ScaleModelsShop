using System.Linq.Expressions;

namespace IS.ScaleModelsShop.Application.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity> GetByIdAsync(Guid id);

    Task<TEntity> AddAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(Guid id);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity> GetEntityByNameAsync(Expression<Func<TEntity, bool>> predicate);

    Task<IEnumerable<TResult>> FilterAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default,
        bool asNoTracking = true);
}