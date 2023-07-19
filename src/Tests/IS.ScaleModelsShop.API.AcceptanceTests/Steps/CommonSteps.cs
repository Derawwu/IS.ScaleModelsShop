using FluentAssertions;
using System.Net;
using IS.ScaleModelsShop.API.AcceptanceTests.Communication;
using IS.ScaleModelsShop.API.AcceptanceTests.Constants;
using TechTalk.SpecFlow;
using IS.ScaleModelsShop.API.Common;
using System.Text.RegularExpressions;
using BoDi;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Categories;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Manufacturers;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Products;
using IS.ScaleModelsShop.Domain.Entities;
using Newtonsoft.Json;
using TechTalk.SpecFlow.Assist;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Steps;

[Binding]
public class CommonSteps : BaseSteps
{
    protected CommonSteps(ScenarioContext scenarioContext,
        IHttpClientService requestHelper,
        IObjectContainer container,
        IManufacturerConfigurator manufacturerConfigurator,
        ICategoryConfigurator categoryConfigurator,
        IProductConfigurator productConfigurator) : base(scenarioContext, requestHelper, container, categoryConfigurator, manufacturerConfigurator, productConfigurator)
    {
    }

    [Given(@"the following Category already exist")]
    public async Task GivenTheFollowingCategoryAlreadyExist(Table categoryData)
    {
        var categoryToCreate = categoryData.CreateInstance<Category>();

        await CategoryConfigurator.CreateAsync(categoryToCreate);
    }

    [Given(@"the following Manufacturer already exist")]
    public async Task GivenTheFollowingManufacturerAlreadyExist(Table manufacturerData)
    {
        var manufaturerToCreate = manufacturerData.CreateInstance<Manufacturer>();

        await ManufacturerConfigurator.CreateAsync(manufaturerToCreate);
    }

    [Given(@"the categories are setup in the database")]
    public async Task GivenTheCategoriesAreSetupInTheDatabase()
    {
        await CategoryConfigurator.SetupDataBaseAsync();
    }

    [Given(@"the manufacturers are setup in the database")]
    public async Task GivenTheManufacturersAreSetupInTheDatabase()
    {
        await ManufacturerConfigurator.SetupDataBaseAsync();
    }

    [Then(@"the ""([^""]*)"" status code is received")]
    public void ThenTheStatusIsReturnedAsync(HttpStatusCode httpStatusCode)
    {
        var actualCode = ScenarioContext.Get<HttpResponseMessage>(TestConstants.Response).StatusCode;
        actualCode.Should().Be(httpStatusCode);
    }

    [Then(@"the ""([^""]*)"" message is returned for the ""([^""]*)"" field")]
    public async Task ThenTheMessageIsReturnedForTheField(string errorMessageType, string fieldName)
    {
        var errorMessageDictionary = this.GetErrorMessageDictionary(fieldName);

        var actualContent = await ScenarioContext.Get<HttpResponseMessage>(TestConstants.Response).Content
            .ReadAsStringAsync();

        string[] errorMessage = { errorMessageDictionary[errorMessageType] };

        if (errorMessageType.Equals("TooManyCharacters") || errorMessageType.Equals("Number of characters not in allowed range"))
        {
            this.ErrorsRegexAssertion(actualContent, errorMessage, fieldName);
        }
        else
        {
            this.ErrorsAssertion(actualContent, errorMessage, fieldName);
        }
    }

    [Then(@"the ""([^""]*)"" error message returned for the (Category|Product|Manufacturer)")]
    public async Task ThenTheErrorMessageReturnedForTheManufacturer(string errorMessageType, string entityType)
    {
        var actualContent = await ScenarioContext.Get<HttpResponseMessage>(TestConstants.Response).Content
            .ReadAsStringAsync();

        var errorMessageDictionary = this.GetErrorMessageDictionaryForEntity(entityType);
        var errorMessage =  errorMessageDictionary[errorMessageType];

        var actualData = JsonConvert.DeserializeObject<ExceptionResponse>(actualContent);

        actualData.Detail.Should().Be(errorMessage);
    }

    [Then(@"the ExtraField field is not present in the (Manufacturer|Category|Product) response body( after update|)")]
    public async Task ThenTheExtraFieldFieldIsNotPresentInTheResponseBody(string entityType, string updated)
    {
        if (string.IsNullOrEmpty(updated))
        {
            var actualContent = await ScenarioContext.Get<HttpResponseMessage>(TestConstants.Response).Content
                .ReadAsStringAsync();

            actualContent.Should().NotContain("ExtraField", "Expected content should not contain ExtraField field");
        }
        else
        {
            var urlForGet = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri;

            urlForGet += entityType switch
            {
                "Manufacturer" => PathConstants.ManufacturersUrl,
                "Category" => PathConstants.CategoriesUrl,
                "Product" => throw new NotImplementedException(),
                _ => throw new ArgumentException($"Provided entity type with name {entityType} is not valid."),
            };
            var response = await RequestHelper.SendRequestAsync(HttpMethod.Get, urlForGet);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().NotContain("ExtraField", "Expected content should not contain ExtraField field");
        }
    }

    private void ErrorsAssertion(string actualContent, string[] errorMessage, string field)
    {
        var actualData = JsonConvert.DeserializeObject<ExceptionResponse>(actualContent);

        actualData.InvalidParameters.First(x => x.Name.Trim('$', '.') == field).Name.Should().Contain(field, "No key in the dictionary");
        actualData.InvalidParameters.First(x => x.Name.Trim('$', '.') == field).Reason.Should().Contain(errorMessage[0], "Unexpected error in response");
    }

    private void ErrorsRegexAssertion(string actualContent, string[] errorMessage, string field)
    {
        var actualData = JsonConvert.DeserializeObject<ExceptionResponse>(actualContent);

        actualData.InvalidParameters.First(x => x.Name.Trim('$', '.') == field).Name.Should().Contain(field, "No key in the dictionary");
        Regex.IsMatch(actualData.InvalidParameters.First(x => x.Name.Trim('$', '.') == field).Reason, errorMessage[0]).Should().BeTrue("Expected data " + errorMessage[0] + "\r\nActual data " + actualData.InvalidParameters.First().Reason);
    }
}