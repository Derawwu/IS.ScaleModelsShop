using IS.ScaleModelsShop.Domain.Entities;

namespace IS.ScaleModelsShop.Application.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        //Task<Category> GetCategoryProductsAsync(string categoryName);
    }
}