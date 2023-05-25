using System.Linq.Expressions;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Common;
using IS.ScaleModelsShop.Domain.Contracts;
using IS.ScaleModelsShop.Domain.Entities;
using IS.ScaleModelsShop.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IS.ScaleModelsShop.Infrastructure.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly IDateTime _dateTime;

    public ProductRepository(AppDbContext appDbContext, IDateTime dateTimeService) : base(
        appDbContext, dateTimeService)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        _dateTime = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
    }

    public async Task<IEnumerable<Product>> GetPaginatedProductsAsync(int pageNumber, int pageSize)
    {
        return await _appDbContext.Products
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).AsNoTracking()
            .ToListAsync();
    }

    public override async Task UpdateAsync(Product entity)
    {
        if (entity is AuditableEntity auditableEntity) auditableEntity.LastModifiedDate = _dateTime.UtcNow;

        _appDbContext.Products.Update(entity);
        await _appDbContext.SaveChangesAsync();

        var categoryProducts = _appDbContext.ProductCategory.Where(cp => cp.LinkedProductId == entity.Id);

        foreach (var categoryProduct in categoryProducts)
        {
            categoryProduct.LinkedCategoryId = entity.CategoryId;
        }

        await _appDbContext.SaveChangesAsync();
    }

    public override async Task DeleteAsync(Guid id)
    {
        var entityToRemove = await _appDbContext.Products.FindAsync(id);
        var categoryProductsToRemove = _appDbContext.ProductCategory.Where(cp => cp.LinkedProductId == entityToRemove.Id);

        _appDbContext.ProductCategory.RemoveRange(categoryProductsToRemove);
        _appDbContext.Remove(entityToRemove);
        await _appDbContext.SaveChangesAsync();
    }

    public override async Task<IEnumerable<TResult>> FilterAsync<TResult>(
        Expression<Func<Product, bool>> predicate,
        Expression<Func<Product, TResult>> selector,
        CancellationToken cancellationToken = default,
        bool asNoTracking = true)
    {
        var query = asNoTracking ? _appDbContext.Products.AsNoTracking() : _appDbContext.Products;

        var result = await query
            .Include(x => x.ProductCategory).ThenInclude(x => x.Category)
            .Where(predicate)
            .Select(selector)
            .ToListAsync(cancellationToken);

        return result;
    }
}