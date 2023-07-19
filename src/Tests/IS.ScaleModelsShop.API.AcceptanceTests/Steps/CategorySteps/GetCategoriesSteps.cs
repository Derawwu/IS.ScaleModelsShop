using BoDi;
using FluentAssertions;
using IS.ScaleModelsShop.API.AcceptanceTests.Communication;
using IS.ScaleModelsShop.API.AcceptanceTests.Constants;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Categories;
using IS.ScaleModelsShop.API.Contracts.Category;
using IS.ScaleModelsShop.Domain.Entities;
using Newtonsoft.Json;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Steps.CategorySteps;

[Binding]
public class GetCategoriesSteps : BaseSteps
{
    protected GetCategoriesSteps(ScenarioContext scenarioContext,
        IHttpClientService requestHelper,
        ICategoryConfigurator categoryConfigurator,
        IObjectContainer container) : base(scenarioContext, requestHelper, categoryConfigurator, container)
    {
    }


    [When(@"the user sends the (GET|) request for Categories")]
    public async Task WhenTheUserSendsTheGETRequestForCategories(HttpMethod requestType)
    {
        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.CategoriesUrl;

        var response = await RequestHelper.SendRequestAsync(requestType, url);

        ScenarioContext.Add(TestConstants.Response, response);
    }

    [Then(@"all categories are returned")]
    public async Task ThenAllCategoriesAreReceived()
    {
        var response = await ScenarioContext.Get<HttpResponseMessage>(TestConstants.Response).Content
            .ReadAsStringAsync();

        var actualData = JsonConvert.DeserializeObject<List<CategoryModel>>(response);

        var expectedData = ScenarioContext.Get<List<Category>>(TestConstants.CreatedCategoriesKey);

        actualData.Should().BeEquivalentTo(expectedData, options => options
            .Excluding(x => x.CreatedBy)
            .Excluding(x => x.CreatedDate)
            .Excluding(x => x.LastModifiedBy)
            .Excluding(x => x.LastModifiedDate)
            .Excluding(x => x.ProductCategory));
    }

    [Then(@"no categories are returned")]
    public async Task ThenNoCategoriesAreReceived()
    {
        var response = await ScenarioContext.Get<HttpResponseMessage>(TestConstants.Response).Content
            .ReadAsStringAsync();

        var actualData = JsonConvert.DeserializeObject<List<CategoryModel>>(response);

        actualData.Should().BeEquivalentTo(Array.Empty<CategoryModel>());
    }
}