using BoDi;
using IS.ScaleModelsShop.API.AcceptanceTests.Utils;
using NUnit.Framework;
using Testcontainers.MsSql;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Services.Containers;

[Parallelizable]
[TestFixture]
public class ContainerService : IContainerService
{
    private readonly MsSqlContainer _container;

    public string ContainerSqlConnectionString => _container.GetConnectionString();

    public ContainerService(IObjectContainer objectContainer)
    {
        var networkTools = objectContainer.Resolve<INetworkTools>();
        var port = networkTools.GetFreeTcpPort();

        _container = new MsSqlBuilder()
            .WithPortBinding(port)
            .Build();
    }

    public async Task StartContainerAsync()
    {
        await _container.StartAsync();
    }

    public async Task StopContainerAsync()
    {
        await _container.StopAsync();
    }
}