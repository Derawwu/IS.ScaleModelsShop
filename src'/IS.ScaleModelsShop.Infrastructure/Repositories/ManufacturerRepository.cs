using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Contracts;
using IS.ScaleModelsShop.Domain.Entities;
using IS.ScaleModelsShop.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IS.ScaleModelsShop.Infrastructure.Repositories;

public class ManufacturerRepository : Repository<Manufacturer>, IManufacturerRepository
{
    private readonly AppDbContext _appDbContext;

    public ManufacturerRepository(AppDbContext appDbContext, IDateTime dateTimeService) : base(appDbContext,
        dateTimeService)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Manufacturer> GetManufacturerProductsAsync(string manufacturerName)
    {
        var manufacturerId = (await _appDbContext.Manufacturers.SingleAsync(m => m.Name == manufacturerName)).Id;

        var list = await _appDbContext.Manufacturers.Include(m => m.Products).SingleAsync(m => m.Id == manufacturerId);

        return list;
    }
}