using System.Linq.Expressions;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Contracts;
using IS.ScaleModelsShop.Domain.Entities;
using IS.ScaleModelsShop.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IS.ScaleModelsShop.Infrastructure.Repositories;

/// <inheritdoc cref="ICategoryRepository"/>
public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly AppDbContext _appDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryRepository"/>
    /// </summary>
    /// <param name="appDbContext">An instance of database context.</param>
    /// <param name="dateTimeService">>Service for working with time.</param>
    public CategoryRepository(AppDbContext appDbContext, IDateTime dateTimeService) : base(appDbContext,
        dateTimeService)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<TResult>> FilterAsync<TResult>(
        Expression<Func<Category, bool>> predicate,
        Expression<Func<Category, TResult>> selector,
        CancellationToken cancellationToken = default,
        bool asNoTracking = true)
    {
        var query = asNoTracking ? _appDbContext.Categories.AsNoTracking() : _appDbContext.Categories;

        var result = await query
            .Include(x => x.ProductCategory).ThenInclude(x => x.Product)
            .Where(predicate)
            .Select(selector)
            .ToListAsync(cancellationToken);

        return result;
    }
}