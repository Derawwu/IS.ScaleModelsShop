using IS.ScaleModelsShop.Domain.Entities;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Categories;

public interface ICategoryConfigurator
{
    Task<Category> CreateAsync(Category category);

    Task<IEnumerable<Category>> GetAllAsync();

    Task RemoveAsync(Guid id);

    Task RemoveAllAsync();

    Task<Category> GetByIdAsync(Guid id);

    Task SetupDataBaseAsync();
}