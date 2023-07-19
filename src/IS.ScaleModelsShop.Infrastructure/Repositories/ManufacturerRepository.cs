using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Contracts;
using IS.ScaleModelsShop.Domain.Entities;
using IS.ScaleModelsShop.Infrastructure.Context;

namespace IS.ScaleModelsShop.Infrastructure.Repositories;

/// <inheritdoc cref="IManufacturerRepository"/>
public class ManufacturerRepository : Repository<Manufacturer>, IManufacturerRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ManufacturerRepository"/>
    /// </summary>
    /// <param name="appDbContext">An instance of database context.</param>
    /// <param name="dateTimeService">>Service for working with time.</param>
    public ManufacturerRepository(AppDbContext appDbContext, IDateTime dateTimeService) : base(appDbContext,
        dateTimeService)
    {
    }
}