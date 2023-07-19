using BoDi;
using IS.ScaleModelsShop.API.AcceptanceTests.Extenions;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Containers;
using IS.ScaleModelsShop.API.AcceptanceTests.Utils;
using IS.ScaleModelsShop.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Services.WebServer;

public class WebServerFactory : IWebServerFactory
{
    private readonly IContainerService _containerService;
    //private readonly SemaphoreSlim _semaphore;
    private readonly INetworkTools _networkTools;

    private WebApplicationFactory<Program> _application;
    private HttpClient? _httpClient;

    public WebServerFactory(IObjectContainer objectContainer)
    {
        //_semaphore = new SemaphoreSlim(1);

        _containerService = objectContainer.Resolve<IContainerService>();
        _networkTools = objectContainer.Resolve<INetworkTools>();
    }

    public HttpClient GetHttpClient()
    {
        if (_httpClient == null)
        {
            CreateWebHostFactory();
        }

        return _httpClient;
    }

    public void DisposeHttpClient()
    {
        _application.Dispose();
        _httpClient.Dispose();
    }

    private void CreateWebHostFactory()
    {
        var sqlConnectionString = _containerService.ContainerSqlConnectionString;

        _application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveDbContext();
                services.AddDbContext<AppDbContext>(options => options.UseSqlServer(sqlConnectionString));

                services.AddConfigurationSource();
            });
        });

        _httpClient = _application.CreateClient();
        _httpClient.BaseAddress = new Uri($"http://localhost:{_networkTools.GetFreeTcpPort()}/");
    }
}