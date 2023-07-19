using IS.ScaleModelsShop.Domain.Entities;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Products;

public interface IProductConfigurator
{
    Task<Product> CreateAsync(Product product);

    Task<IEnumerable<Product>> GetAllAsync();

    Task RemoveAsync(Guid id);

    Task RemoveAllAsync();

    Task<Product> GetByIdAsync(Guid id);

    Task SetupDataBaseAsync();
}