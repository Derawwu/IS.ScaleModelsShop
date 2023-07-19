using BoDi;
using FluentAssertions;
using IS.ScaleModelsShop.API.AcceptanceTests.Communication;
using IS.ScaleModelsShop.API.AcceptanceTests.Constants;
using IS.ScaleModelsShop.API.AcceptanceTests.Extenions;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Categories;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Formatters;
using IS.ScaleModelsShop.API.Contracts.Category;
using IS.ScaleModelsShop.Application.Features.Categories.Commands.CreateCategory;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Steps.CategorySteps;

[Binding]
public class CreateCategorySteps : BaseSteps
{
    private readonly IStringContentFormatter _formatter;

    protected CreateCategorySteps(
        ScenarioContext scenarioContext,
        IHttpClientService requestHelper,
        IStringContentFormatter formatter,
        ICategoryConfigurator categoryConfigurator,
        IObjectContainer container) : base(scenarioContext, requestHelper, categoryConfigurator, container)
    {
        _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
    }

    [When(@"the user sends the (POST|) request for Category with data in the request body")]
    public async Task WhenTheUserSendsThePOSTRequestForCategoryWithDataInTheRequestBody(HttpMethod requestType, Table categoryData)
    {
        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.CategoriesUrl;

        var data = categoryData.CreateInstance<CreateCategoryCommand>();
        var content = _formatter.CreateStringContent(data);

        var response = await RequestHelper.SendRequestAsync(requestType, url, content);

        ScenarioContext.Add(TestConstants.Response, response);
    }

    [When(@"the user sends the (POST|) request for Category with ""([^""]*)"" as the Name property")]
    public async Task WhenTheUserSendsThePOSTRequestForCategoryWithNullAsTheNameProperty(HttpMethod requestType, string nameValue)
    {
        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.CategoriesUrl;

        nameValue = nameValue == "empty string" ? string.Empty : nameValue;
        nameValue = nameValue == "null" ? null : nameValue;

        var content = _formatter.CreateStringContent(new CreateCategoryCommand { Name = nameValue });

        var response = await RequestHelper.SendRequestAsync(requestType, url, content);

        ScenarioContext.Add(TestConstants.Response, response);
    }

    [When(@"the user sends the (POST|) request for a Category with the Name property value of more than (.*) characters")]
    public async Task WhenTheUserSendsThePOSTRequestForCategoryWithNamePropertyValueMoreThanCharacters(HttpMethod requestType, int charactersCount)
    {
        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.CategoriesUrl;
        var content = _formatter.CreateStringContent(new CreateCategoryCommand { Name = new string('a', charactersCount + 1) });

        var response = await RequestHelper.SendRequestAsync(requestType, url, content);

        ScenarioContext.Add(TestConstants.Response, response);
    }

    [When(@"the user sends the (POST|) request for Category with extra filed in the request body")]
    public async Task WhenTheUserSendsThePOSTRequestForCategoryWithExtraFiledInTheRequestBody(HttpMethod requestType, Table table)
    {
        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.CategoriesUrl;

        var data = table.CreateInstance<CreateCategoryCommand>();
        var dataModel = new
        {
            data.Name,
            ExtraField = "ExtraField"
        };

        var content = _formatter.CreateStringContent(dataModel);

        var response = await RequestHelper.SendRequestAsync(requestType, url, content);

        ScenarioContext.Add(TestConstants.Response, response);
    }

    [When(@"the user sends the (POST|) request for Category without request body")]
    public async Task WhenTheUserSendsThePOSTRequestForCategoryWithoutRequestBody(HttpMethod requestType)
    {
        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.CategoriesUrl;

        var response = await RequestHelper.SendRequestAsync(requestType, url);

        ScenarioContext.Add(TestConstants.Response, response);
    }

    [Then(@"the Category response body matches the expected data")]
    public async Task ThenTheCategoryResponseBodyMatchesTheExpectedData()
    {
        var response = await ScenarioContext.Get<HttpResponseMessage>(TestConstants.Response).Content
            .ReadAsStringAsync();

        var actualData = JsonConvert.DeserializeObject<CategoryModel>(response);
        var expectedData = await CategoryConfigurator.GetByIdAsync(actualData.Id);

        actualData.Name.Should().Be(expectedData.Name);
    }
}