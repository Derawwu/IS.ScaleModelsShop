using BoDi;
using IS.ScaleModelsShop.API.AcceptanceTests.Communication;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Containers;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Categories;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.FilesProvider;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Manufacturers;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Products;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Formatters;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.WebServer;
using IS.ScaleModelsShop.API.AcceptanceTests.Utils;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Contracts;
using IS.ScaleModelsShop.Infrastructure.Common;
using IS.ScaleModelsShop.Infrastructure.Context;
using IS.ScaleModelsShop.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Extenions;

public static class ServiceRegister
{
    public static async Task RegisterCommonServices(this IObjectContainer objectContainer)
    {
        objectContainer.AddConteinerizedDatabase();
        objectContainer.RegisterFactoryAs<IWebServerFactory>(oc => new WebServerFactory(oc));

        objectContainer.RegisterFactoryAs<IHttpClientService>(oc =>
        {
            var serverFactory = oc.Resolve<IWebServerFactory>();

            var client = serverFactory.GetHttpClient();

            return new HttpClientService(client);
        });

        objectContainer.RegisterTypeAs<StringContentFormatter, IStringContentFormatter>();
        objectContainer.RegisterTypeAs<FileProviderService, IFileProviderService>();
        objectContainer.RegisterTypeAs<NetworkTools, INetworkTools>();
        await objectContainer.AddDbContext();
    }

    public static void RegisterScenarioServices(this IObjectContainer objectContainer)
    {
        objectContainer.RegisterTypeAs<CategoryConfigurator, ICategoryConfigurator>();
        objectContainer.RegisterTypeAs<CategoryRepository, ICategoryRepository>();
        objectContainer.RegisterTypeAs<ManufacturerConfigurator, IManufacturerConfigurator>();
        objectContainer.RegisterTypeAs<ManufacturerRepository, IManufacturerRepository>();
        objectContainer.RegisterTypeAs<ProductConfigurator, IProductConfigurator>();
        objectContainer.RegisterTypeAs<ProductRepository, IProductRepository>();
        objectContainer.RegisterTypeAs<DateTimeService, IDateTime>();
    }

    private static async Task AddDbContext(this IObjectContainer objectContainer)
    {
        var containerService = objectContainer.Resolve<IContainerService>();

        await containerService.StartContainerAsync();

        objectContainer.RegisterFactoryAs(oc =>
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            contextOptionsBuilder.UseSqlServer(containerService.ContainerSqlConnectionString);

            var contextInstance = new AppDbContext(contextOptionsBuilder.Options);

            contextInstance.Database.EnsureCreated();

            return contextInstance;
        });
    }

    private static void AddConteinerizedDatabase(this IObjectContainer objectContainer)
    {
        objectContainer.RegisterFactoryAs<IContainerService>(oc => new ContainerService(oc));
    }

    public static void AddConfigurationSource(this IServiceCollection services)
    {
        services.RemoveAll(typeof(IConfiguration));

        var configurationRoot = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var configuration = configurationRoot as IConfiguration;

        services.AddSingleton(configuration);
    }


    public static void RemoveDbContext(this IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
        if (descriptor != null) services.Remove(descriptor);
    }
}