using BoDi;
using FluentAssertions;
using IS.ScaleModelsShop.API.AcceptanceTests.Communication;
using IS.ScaleModelsShop.API.AcceptanceTests.Constants;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Products;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Formatters;
using IS.ScaleModelsShop.API.Contracts.Product;
using IS.ScaleModelsShop.Application.Features.Products.Commands.CreateProduct;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Steps.Products;

[Binding]
public class CreateProductSteps : BaseSteps
{
    private readonly IStringContentFormatter _formatter;

    protected CreateProductSteps(ScenarioContext scenarioContext,
        IHttpClientService requestHelper,
        IProductConfigurator productConfigurator,
        IObjectContainer container,
        IStringContentFormatter formatter) : base(scenarioContext, requestHelper, productConfigurator, container)
    {
        _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
    }

    [When(@"the user sends the (POST|) request for Product with data in the request body")]
    public async Task WhenTheUserSendsThePOSTRequestForProductWithDataInTheRequestBody(HttpMethod requestType, Table productData)
    {
        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.ProductUrl;

        var data = productData.CreateInstance<CreateProductCommand>();
        var content = _formatter.CreateStringContent(data);

        var response = await RequestHelper.SendRequestAsync(requestType, url, content);

        ScenarioContext.Add(TestConstants.Response, response);
    }

    [Then(@"the following Product is updated in the database")]
    public async Task ThenTheFollowingProductIsUpdatedInTheDatabase(Table expectedProductData)
    {
        var response = await ScenarioContext.Get<HttpResponseMessage>(TestConstants.Response).Content
            .ReadAsStringAsync();

        var actualData = JsonConvert.DeserializeObject<ProductModel>(response);
        var expectedData = await ProductConfigurator.GetByIdAsync(actualData.Id);

        actualData.Name.Should().Be(expectedData.Name);
        actualData.Description.Should().Be(expectedData.Description);
        actualData.CategoryId.Should().Be(expectedData.CategoryId);
        actualData.ManufacturerId.Should().Be(expectedData.ManufacturerId);
    }
}