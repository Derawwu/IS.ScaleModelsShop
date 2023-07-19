using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IS.ScaleModelsShop.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static async Task<TEntity>? GetById<TEntity>(this IQueryable<TEntity> query, Guid id)
    {
        // This expression is lambad : e => e.Id == id
        var parameter = Expression.Parameter(typeof(TEntity));
        var left = Expression.Property(parameter, "Id");
        var right = Expression.Constant(id);
        var equal = Expression.Equal(left, right);
        var byId = Expression.Lambda<Func<TEntity, bool>>(equal, parameter);

        return await query.SingleOrDefaultAsync(byId);
    }
}