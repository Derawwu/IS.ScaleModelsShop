using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Common;
using IS.ScaleModelsShop.Domain.Contracts;
using IS.ScaleModelsShop.Domain.Entities;
using IS.ScaleModelsShop.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace IS.ScaleModelsShop.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IDateTime _dateTime;

        public ProductRepository(AppDbContext appDbContext, IDateTime dateTimeService, IDateTime dateTime) : base(appDbContext, dateTimeService)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
            _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
        }

        public async Task<List<Product>> GetPaginatedProductsAsync(int pageNumber, int pageSize)
        {
            return await _appDbContext.Products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).AsNoTracking()
                .ToListAsync();
        }

        public override async Task UpdateAsync(Product entity)
        {
            if (entity is AuditableEntity auditableEntity)
            {
                auditableEntity.LastModifiedDate = _dateTime.UtcNow;
            }

            var productCategory = (await _appDbContext.Products
                .Include(x => x.ProductCategory)
                .Where(x => x.Id == entity.Id)
                .SingleAsync()).ProductCategory.Single();



            await using (var transaction = await _appDbContext.Database.BeginTransactionAsync())
            {
                _appDbContext.Update(entity);
                _appDbContext.ProductCategory.Remove(productCategory);
                await _appDbContext.SaveChangesAsync();

                productCategory.LinkedCategoryId = entity.CategoryId;
                await _appDbContext.ProductCategory.AddAsync(productCategory);
                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }

        public override async Task DeleteAsync(Guid id)
        {
            var entityToRemove = await _appDbContext.Products.FindAsync(id);

            if (entityToRemove is AuditableEntity auditableEntity)
            {
                auditableEntity.LastModifiedDate = _dateTime.UtcNow;
            }

            var productCategory = (await _appDbContext.Products
                .Include(x => x.ProductCategory)
                .Where(x => x.Id == entityToRemove.Id)
                .SingleAsync()).ProductCategory.Single();

            await using (var transaction = await _appDbContext.Database.BeginTransactionAsync())
            {
                _appDbContext.ProductCategory.Remove(productCategory);
                _appDbContext.Remove(entityToRemove);

                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
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
}