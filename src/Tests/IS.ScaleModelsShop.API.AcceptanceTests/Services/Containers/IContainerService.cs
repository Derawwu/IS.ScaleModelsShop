namespace IS.ScaleModelsShop.API.AcceptanceTests.Services.Containers;

public interface IContainerService
{
    public string ContainerSqlConnectionString { get; }

    Task StartContainerAsync();

    Task StopContainerAsync();
}