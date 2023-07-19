using BoDi;
using IS.ScaleModelsShop.API.AcceptanceTests.Extenions;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Categories;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Manufacturers;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Products;
using TechTalk.SpecFlow;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Hooks;

[Binding]
public class ScenarioHooks
{
    private readonly IObjectContainer _container;

    public ScenarioHooks(IObjectContainer container)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container));
    }

    [BeforeScenario(Order = 0)]
    public void RegisterScenarioServices()
    {
        _container.RegisterScenarioServices();
    }

    [AfterScenario(Order = 0)]
    public async Task CleanUpDb()
    {
        var categoryConfigurator = _container.Resolve<ICategoryConfigurator>();
        var manufacturerConfigurator = _container.Resolve<IManufacturerConfigurator>();
        var productConfigurator = _container.Resolve<IProductConfigurator>();

        await categoryConfigurator.RemoveAllAsync();
        await manufacturerConfigurator.RemoveAllAsync();
        await productConfigurator.RemoveAllAsync();
    }
}