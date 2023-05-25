using System.Linq.Expressions;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Contracts;
using IS.ScaleModelsShop.Domain.Entities;
using IS.ScaleModelsShop.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IS.ScaleModelsShop.Infrastructure.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly AppDbContext _appDbContext;

    public CategoryRepository(AppDbContext appDbContext, IDateTime dateTimeService) : base(appDbContext,
        dateTimeService)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

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