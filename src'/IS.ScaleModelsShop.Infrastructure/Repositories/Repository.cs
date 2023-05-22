using System.Linq.Expressions;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Common;
using IS.ScaleModelsShop.Domain.Contracts;
using IS.ScaleModelsShop.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IS.ScaleModelsShop.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly AppDbContext _appDbContext;
    private readonly IDateTime _dateTime;

    public Repository(AppDbContext appDbContext, IDateTime dateTime)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _appDbContext.Set<TEntity>()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        return await _appDbContext.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        if (entity is AuditableEntity auditableEntity)
        {
            auditableEntity.CreatedDate = _dateTime.UtcNow;
            auditableEntity.LastModifiedDate = _dateTime.UtcNow;
        }

        await _appDbContext.Set<TEntity>().AddAsync(entity);
        await _appDbContext.SaveChangesAsync();

        return entity;
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        if (entity is AuditableEntity auditableEntity) auditableEntity.LastModifiedDate = _dateTime.UtcNow;

        var entry = _appDbContext.Set<TEntity>().Update(entity);
        await _appDbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await _appDbContext.Set<TEntity>().FindAsync(id);
        _appDbContext.Set<TEntity>().Remove(entity);

        await _appDbContext.SaveChangesAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _appDbContext.Set<TEntity>().AnyAsync(predicate);
    }

    public async Task<TEntity> GetEntityByNameAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entity = await _appDbContext.Set<TEntity>().Where(predicate).FirstOrDefaultAsync();

        return entity;
    }

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
}