using BoDi;
using IS.ScaleModelsShop.API.AcceptanceTests.Extenions;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Containers;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.WebServer;
using TechTalk.SpecFlow;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Hooks;

[Binding]
public sealed class GlobalHooks
{
    [BeforeFeature(Order = 0)]
    public static async Task RegisterServices(IObjectContainer container)
    {
        await container.RegisterCommonServices();
    }

    [AfterFeature(Order = 0)]
    public static async Task AfterFeature(IContainerService containerService, IWebServerFactory serverFactory)
    {
        await containerService.StopContainerAsync();

        serverFactory.DisposeHttpClient();
    }
}