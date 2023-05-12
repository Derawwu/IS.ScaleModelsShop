using IS.ScaleModelsShop.Domain.Entities;

namespace IS.ScaleModelsShop.Application.Repositories
{
    public interface IManufacturerRepository : IRepository<Manufacturer>
    {
        Task<Manufacturer> GetManufacturerProductsAsync(string manufacturerName);
    }
}