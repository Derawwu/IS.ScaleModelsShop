using IS.ScaleModelsShop.Domain.Entities;

namespace IS.ScaleModelsShop.Application.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetPaginatedProductsAsync (int pageNumber, int pageSize);
    }
}