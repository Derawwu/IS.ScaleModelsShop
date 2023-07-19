using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Contracts;
using IS.ScaleModelsShop.Infrastructure.Common;
using IS.ScaleModelsShop.Infrastructure.Constants;
using IS.ScaleModelsShop.Infrastructure.Context;
using IS.ScaleModelsShop.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IS.ScaleModelsShop.Infrastructure.Extensions;

/// <summary>
/// Extension methods for <see cref="IServiceCollection" /> for services dependency injection.
/// </summary>
public static class ServiceCollectionDiExtensions
{
    /// <summary>
    /// Injects services.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection" />.</param>
    /// <param name="configuration"><see cref="IConfiguration"/>An instance, containing application configurations.</param>
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString(SqlConstants.ScaleModelsSqlServerConnectionString)));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}