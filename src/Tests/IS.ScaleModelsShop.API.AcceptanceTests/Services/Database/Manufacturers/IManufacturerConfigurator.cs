using IS.ScaleModelsShop.Domain.Entities;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Manufacturers;

public interface IManufacturerConfigurator
{
    Task<Manufacturer> CreateAsync(Manufacturer manufacturer);

    Task<IEnumerable<Manufacturer>> GetAllAsync();

    Task RemoveAsync(Guid id);

    Task RemoveAllAsync();

    Task<Manufacturer> GetByIdAsync(Guid id);

    Task SetupDataBaseAsync();
}