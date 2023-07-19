using System.Linq.Expressions;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Common;
using IS.ScaleModelsShop.Domain.Contracts;
using IS.ScaleModelsShop.Infrastructure.Context;
using IS.ScaleModelsShop.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace IS.ScaleModelsShop.Infrastructure.Repositories;

/// <inheritdoc cref="IRepository{TEntity}"/>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly AppDbContext _appDbContext;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository"/>
    /// </summary>
    /// <param name="appDbContext">An instance of database context.</param>
    /// <param name="dateTime">>Service for working with time.</param>
    public Repository(AppDbContext appDbContext, IDateTime dateTime)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _appDbContext.Set<TEntity>()
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<TEntity?> GetByIdAsync(Guid id, bool asNoTracking = false)
    {
        var query = asNoTracking
            ? _appDbContext.Set<TEntity>().AsNoTracking()
            : _appDbContext.Set<TEntity>();

        return await query.GetById(id);
    }

    /// <inheritdoc />
    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        if (entity is AuditableEntity auditableEntity)
        {
            auditableEntity.CreatedDate = _dateTime.UtcNow;
            //auditableEntity.LastModifiedDate = _dateTime.UtcNow;
        }

        await _appDbContext.Set<TEntity>().AddAsync(entity);
        await _appDbContext.SaveChangesAsync();

        return entity;
    }

    /// <inheritdoc />
    public virtual async Task UpdateAsync(TEntity entity)
    {
        if (entity is AuditableEntity auditableEntity) auditableEntity.LastModifiedDate = _dateTime.UtcNow;

        _appDbContext.Set<TEntity>().Update(entity);
        await _appDbContext.SaveChangesAsync();
    }

    /// <inheritdoc />
    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await _appDbContext.Set<TEntity>().FindAsync(id);
        _appDbContext.Set<TEntity>().Remove(entity);

        await _appDbContext.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _appDbContext.Set<TEntity>().AnyAsync(predicate);
    }

    /// <inheritdoc />
    public async Task<TEntity> GetEntityByNameAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entity = await _appDbContext.Set<TEntity>().Where(predicate).FirstOrDefaultAsync();

        return entity;
    }

    /// <inheritdoc />
    public virtual async Task<IEnumerable<TResult>> FilterAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default,
        bool asNoTracking = true)
    {
        var entities = _appDbContext.Set<TEntity>();

        return asNoTracking
            ? await entities.AsNoTracking().Where(predicate).Select(selector).ToListAsync(cancellationToken)
            : await entities.Where(predicate).Select(selector).ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PaginatedCollection<TEntity>> GetByQueryAsync(EntityOptionQuery<TEntity> entityOptionQuery, CancellationToken cancellationToken = default,
        bool asNoTracking = true)
    {
        var entities = _appDbContext.Set<TEntity>();

        var source = asNoTracking
            ? entities.AsNoTracking()
            : entities;

        var count = await entityOptionQuery.TotalCountQuery(source).CountAsync(cancellationToken);

        var paginatedItems = await entityOptionQuery.TakeEntitiesQuery(source).ToListAsync(cancellationToken);

        return new PaginatedCollection<TEntity>(paginatedItems, count);
    }

    /// <inheritdoc />
    public async Task RemoveAllAsync()
    {
        _appDbContext.Set<TEntity>().RemoveRange(_appDbContext.Set<TEntity>());
        await _appDbContext.SaveChangesAsync();
    }
}