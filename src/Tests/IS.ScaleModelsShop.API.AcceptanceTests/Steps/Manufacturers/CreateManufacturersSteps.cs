using BoDi;
using FluentAssertions;
using IS.ScaleModelsShop.API.AcceptanceTests.Communication;
using IS.ScaleModelsShop.API.AcceptanceTests.Constants;
using IS.ScaleModelsShop.API.AcceptanceTests.Extenions;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Manufacturers;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Formatters;
using IS.ScaleModelsShop.API.Contracts.Manufacturer;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Steps.Manufacturers;

[Binding]
public class CreateManufacturersSteps : BaseSteps
{

    private readonly IStringContentFormatter _formatter;

    protected CreateManufacturersSteps(
        ScenarioContext scenarioContext,
        IHttpClientService requestHelper,
        IManufacturerConfigurator manufacturerConfigurator,
        IObjectContainer container,
        IStringContentFormatter formatter) : base(scenarioContext, requestHelper, manufacturerConfigurator, container)
    {
        _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
    }

    [When(@"the user sends the (POST|) request for Manufacturer with data in the request body")]
    public async Task WhenTheUserSendsThePOSTRequestForManufacturerWithDataInTheRequestBody(HttpMethod requestType, Table manufacturerData)
    {
        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.ManufacturersUrl;

        var data = manufacturerData.CreateInstance<CreateManufacturerCommand>();
        var content = _formatter.CreateStringContent(data);

        var response = await RequestHelper.SendRequestAsync(requestType, url, content);

        ScenarioContext.Add(TestConstants.Response, response);
    }

    [When(@"the user sends the (POST|) request for Manufacturer with ""([^""]*)"" as the Name property")]
    public async Task WhenTheUserSendsThePOSTRequestForManufacturerWithAsTheNameProperty(HttpMethod requestType, string nameValue)
    {
        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.ManufacturersUrl;

        nameValue = nameValue == "empty string" ? string.Empty : nameValue;
        nameValue = nameValue == "null" ? null : nameValue;

        var content = _formatter.CreateStringContent(new CreateManufacturerCommand() { Name = nameValue });

        var response = await RequestHelper.SendRequestAsync(requestType, url, content);

        ScenarioContext.Add(TestConstants.Response, response);
    }

    [When(@"the user sends the (POST|) request for Manufacturer with ""([^""]*)"" as the Website property")]
    public async Task WhenTheUserSendsThePOSTRequestForManufacturerWithAsTheWebsiteProperty(HttpMethod requestType, string websiteValue)
    {
        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.ManufacturersUrl;

        var content = _formatter.CreateStringContent(new CreateManufacturerCommand() { Name = "TestManufacturer", Website = websiteValue });

        var response = await RequestHelper.SendRequestAsync(requestType, url, content);

        ScenarioContext.Add(TestConstants.Response, response);
    }

    [When(@"the user sends the (POST|) request for Manufacturer filling (more|less) than (.*) characters into the (Website|Name) property")]
    public async Task WhenTheUserSendsThePOSTRequestForManufacturerFillingMoreThanCharactersIntoTheWebsiteProperty(HttpMethod requestType,
        string moreOrLess, int charactersNumber, string fieldName)
    {
        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.ManufacturersUrl;

        charactersNumber = moreOrLess == "more" ? charactersNumber + 1 : charactersNumber - 1;

        var content = _formatter.CreateStringContent(this.CreateRequestBody(charactersNumber, fieldName));

        var response = await RequestHelper.SendRequestAsync(requestType, url, content);

        ScenarioContext.Add(TestConstants.Response, response);
    }

    [When(@"the user sends the (POST|) request for Manufacturer without the request body")]
    public async Task WhenTheUserSendsThePOSTRequestForManufacturerWithoutTheRequestBody(HttpMethod requestType)
    {
        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.ManufacturersUrl;

        var response = await RequestHelper.SendRequestAsync(requestType, url);

        ScenarioContext.Add(TestConstants.Response, response);
    }

    [When(@"the user sends the (POST|) request for Manufacturer with extra field in the request body")]
    public async Task WhenTheUserSendsThePOSTRequestForManufacturerWithExtraFieldInTheRequestBody(HttpMethod requestType, Table manufacturerData)
    {
        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.ManufacturersUrl;

        var data = manufacturerData.CreateInstance<CreateManufacturerCommand>();
        var dataModel = new
        {
            data.Name,
            data.Website,
            ExtraField = "ExtraField"
        };

        var content = _formatter.CreateStringContent(dataModel);

        var response = await RequestHelper.SendRequestAsync(requestType, url, content);

        ScenarioContext.Add(TestConstants.Response, response);
    }

    [When(@"the user sends the (POST|) request for Manufacturer with empty request body")]
    public async Task WhenTheUserSendsThePOSTRequestForManufacturerWithEmptyRequestBody(HttpMethod requestType)
    {
        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.ManufacturersUrl;

        var content = _formatter.CreateStringContent(new object());

        var response = await RequestHelper.SendRequestAsync(requestType, url, content);

        ScenarioContext.Add(TestConstants.Response, response);
    }

    [Then(@"the Manufacturer response body matches the expected data")]
    public async Task ThenTheManufacturerResponseBodyMatchesTheExpectedData()
    {
        var response = await ScenarioContext.Get<HttpResponseMessage>(TestConstants.Response).Content
            .ReadAsStringAsync();

        var actualData = JsonConvert.DeserializeObject<ManufacturerModel>(response);
        var expectedData = await ManufacturerConfigurator.GetByIdAsync(actualData.Id);

        actualData.Name.Should().Be(expectedData.Name);
        actualData.Website.Should().Be(expectedData.Website);
    }

    private CreateManufacturerCommand CreateRequestBody(int charactersNumber, string fieldName)
    {
        switch (fieldName)
        {
            case "Website":
                return new CreateManufacturerCommand
                {
                    Name = "TestManufacturer",
                    Website = $"http://{new string('a', charactersNumber)}.com"
                };
            case "Name":
                return new CreateManufacturerCommand
                {
                    Name = new string('a', charactersNumber),
                    Website = "www.example.org"
                };
            default:
                throw new ArgumentException(
                    $"Provided field name '{fieldName}' does not exist for Create Manufacturer model.");
        }
    }
}