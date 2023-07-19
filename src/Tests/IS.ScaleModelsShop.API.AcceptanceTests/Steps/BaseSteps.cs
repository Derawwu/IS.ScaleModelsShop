using BoDi;
using IS.ScaleModelsShop.API.AcceptanceTests.Communication;
using IS.ScaleModelsShop.API.AcceptanceTests.Constants;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Categories;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Manufacturers;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.WebServer;
using IS.ScaleModelsShop.Domain.Configuration;
using NUnit.Framework;
using System.ComponentModel;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Products;
using TechTalk.SpecFlow;

[assembly: Parallelizable(ParallelScope.Fixtures)]
//[assembly: LevelOfParallelism(2)]
namespace IS.ScaleModelsShop.API.AcceptanceTests.Steps;

public class BaseSteps
{
    protected ScenarioContext ScenarioContext { get; }

    protected IHttpClientService RequestHelper { get; }

    protected ICategoryConfigurator CategoryConfigurator { get; }

    protected IManufacturerConfigurator ManufacturerConfigurator { get; }

    protected IProductConfigurator ProductConfigurator { get; }

    protected IObjectContainer Container { get; }

    private HttpClient HttpClient { get; }

    private Dictionary<string, string> SplitField { get; }


    protected BaseSteps(ScenarioContext scenarioContext, IHttpClientService requestHelper,
        IManufacturerConfigurator manufacturerConfigurator, IObjectContainer container)
    {
        ScenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
        RequestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        ManufacturerConfigurator = manufacturerConfigurator ?? throw new ArgumentNullException(nameof(manufacturerConfigurator));
        Container = container ?? throw new ArgumentNullException(nameof(container));

        var serverFactory = container.Resolve<IWebServerFactory>();
        HttpClient = serverFactory.GetHttpClient();
        ScenarioContext[TestConstants.BaseClientAddress] = HttpClient.BaseAddress;
    }

    protected BaseSteps(ScenarioContext scenarioContext, IHttpClientService requestHelper, ICategoryConfigurator categoryConfigurator, IObjectContainer container)
    {
        ScenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
        RequestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        CategoryConfigurator = categoryConfigurator ?? throw new ArgumentNullException(nameof(categoryConfigurator));
        Container = container ?? throw new ArgumentNullException(nameof(container));

        var serverFactory = container.Resolve<IWebServerFactory>();
        HttpClient = serverFactory.GetHttpClient();
        ScenarioContext[TestConstants.BaseClientAddress] = HttpClient.BaseAddress;
    }

    protected BaseSteps(ScenarioContext scenarioContext, IHttpClientService requestHelper, IProductConfigurator productConfigurator, IObjectContainer container)
    {
        ScenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
        RequestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        ProductConfigurator = productConfigurator ?? throw new ArgumentNullException(nameof(productConfigurator));
        Container = container ?? throw new ArgumentNullException(nameof(container));

        var serverFactory = container.Resolve<IWebServerFactory>();
        HttpClient = serverFactory.GetHttpClient();
        ScenarioContext[TestConstants.BaseClientAddress] = HttpClient.BaseAddress;
    }

    protected BaseSteps(ScenarioContext scenarioContext, IHttpClientService requestHelper, IObjectContainer container,
        ICategoryConfigurator categoryConfigurator, IManufacturerConfigurator manufacturerConfigurator, IProductConfigurator productConfigurator)
    {
        ScenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
        RequestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        Container = container ?? throw new ArgumentNullException(nameof(container));
        CategoryConfigurator = categoryConfigurator ?? throw new ArgumentNullException(nameof(categoryConfigurator));
        ManufacturerConfigurator = manufacturerConfigurator ?? throw new ArgumentNullException(nameof(manufacturerConfigurator));
        ProductConfigurator = productConfigurator ?? throw new ArgumentNullException(nameof(productConfigurator));

        var serverFactory = container.Resolve<IWebServerFactory>();
        HttpClient = serverFactory.GetHttpClient();
        ScenarioContext[TestConstants.BaseClientAddress] = HttpClient.BaseAddress;

        this.SplitField = new Dictionary<string, string>()
        {
            ["Name"] = "Name",
            ["Website"] = "Website"
        };
    }

    /// <summary>
    /// Get error message dictionary for requested field.
    /// </summary>
    /// <param name="field">Field.</param>
    /// <param name="value">A value.</param>
    /// <returns>Dictionary of error messages.</returns>
    public Dictionary<string, string> GetErrorMessageDictionary(string field, string? value = null)
    {
        return new Dictionary<string, string>()
        {
            ["Category already exists"] = $"Category with the same '{this.SplitField[field]}' already exists.",
            ["Manufacturer already exists"] = $"Manufacturer with the same '{this.SplitField[field]}' already exists.",
            ["mustNotBeEmpty"] = $"'{this.SplitField[field]}' must not be empty.",
            ["extra field"] = $@"Could not find member '{this.SplitField[field]}' on object of type '[Create]*[Edit]*OrganizationModel'. Path '{this.SplitField[field]}', line \d+, position \d+.",
            ["required"] = $"The {this.SplitField[field]} field is required.",
            ["TooManyCharacters"] = $@"The length of '{this.SplitField[field]}' must be \d+ characters or fewer. You entered \d+ characters.",
            ["Invalid URL"] = $"'{this.SplitField[field]}' is not in the correct format. Expected format - \"http(s)://\" or \"www.\" following with the site name and domain name (e.g., www.example.org)",
            ["Inappropriate domain name"] = $"Provided '{this.SplitField[field]}' has unacceptable domain name( e.g., \".ru\" or \".su\")",
            ["Number of characters not in allowed range"] = $@"'{this.SplitField[field]}' must be between \d+ and \d+ characters. You entered \d+ characters."
        };
    }

    /// <summary>
    /// Get error message dictionary for entity.
    /// </summary>
    /// <param name="field">Field.</param>
    /// <param name="value">A value.</param>
    /// <returns>Dictionary of error messages.</returns>
    public Dictionary<string, string> GetErrorMessageDictionaryForEntity(string entityName)
    {
        return new Dictionary<string, string>()
        {
            ["NotFound"] = $"{entityName} ({Guid.Empty}) is not found"
        };
    }
}