using BoDi;
using FluentAssertions;
using IS.ScaleModelsShop.API.AcceptanceTests.Communication;
using IS.ScaleModelsShop.API.AcceptanceTests.Constants;
using IS.ScaleModelsShop.API.AcceptanceTests.Extenions;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Manufacturers;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Formatters;
using IS.ScaleModelsShop.API.Contracts.Manufacturer;
using IS.ScaleModelsShop.API.Contracts.Manufacturer.UpdateManufacturer;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer;
using IS.ScaleModelsShop.Domain.Entities;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Steps.Manufacturers;

[Binding]
public class UpdateManufacturerSteps : BaseSteps
{
    private readonly IStringContentFormatter _formatter;

    protected UpdateManufacturerSteps(ScenarioContext scenarioContext,
        IHttpClientService requestHelper,
        IManufacturerConfigurator manufacturerConfigurator,
        IObjectContainer container,
        IStringContentFormatter formatter) : base(scenarioContext, requestHelper, manufacturerConfigurator, container)
    {
        _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
    }

    [When(@"the user sends the (PUT|) request for Manufacturer with( non-existing|) ManufacturerId and the following data")]
    public async Task WhenTheUserSendsThePUTRequestsForManufacturerWithManufacturerIdTheFollowingData(HttpMethod requestType, string manufacturerExist, Table manufacturerData)
    {
        var manufacturerId = Guid.Empty;

        if (string.IsNullOrEmpty(manufacturerExist))
        {
            manufacturerId = this.GetIdOfFirstManufacturer();
        }

        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.ManufacturersUrl + "/" + manufacturerId;

        var data = manufacturerData.CreateInstance<CreateManufacturerCommand>();
        var content = _formatter.CreateStringContent(data);

        var response = await RequestHelper.SendRequestAsync(requestType, url, content);

        ScenarioContext.Add(TestConstants.Response, response);
    }

    [When(@"the user sends the (PUT|) request for Manufacturer with ManufacturerId and with extra field in the request body")]
    public async Task WhenTheUserSendsThePUTRequestForManufacturerWithManufacturerIdAndWithExtraFieldInTheRequestBody(HttpMethod requestType, Table manufacturerData)
    {
        var manufacturerId = this.GetIdOfFirstManufacturer();
        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.ManufacturersUrl + "/" + manufacturerId;

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

    [When(@"the user sends the (PUT|) request for Manufacturer (with empty request body|without request body)")]
    public async Task WhenTheUserSendsThePUTRequestForManufacturerWithEmptyRequestBody(HttpMethod requestType, string stateOfRequestBody)
    {
        var manufacturerId = Guid.NewGuid();

        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.ManufacturersUrl + "/" + manufacturerId;

        var content = _formatter.CreateStringContent(new object());

        if(stateOfRequestBody == "with empty request body")
        {
            var response = await RequestHelper.SendRequestAsync(requestType, url, content);
            ScenarioContext.Add(TestConstants.Response, response);
        }
        else
        {
            var response = await RequestHelper.SendRequestAsync(requestType, url);
            ScenarioContext.Add(TestConstants.Response, response);
        }
    }

    [When(@"the user sends the (PUT|) request for Manufacturer filling (more|less) than (.*) characters into the (Website|Name) property")]
    public async Task WhenTheUserSendsThePUTRequestForManufacturerFillingMoreThanCharactersIntoTheWebsiteProperty(HttpMethod requestType, string moreOrLess, int charactersCount, string fieldName)
    {
        var manufacturerId = Guid.NewGuid();

        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.ManufacturersUrl + "/" + manufacturerId;

        charactersCount = moreOrLess == "more" ? charactersCount + 1 : charactersCount - 1;

        var content = _formatter.CreateStringContent(this.CreateRequestBodyWithIncorrectCharactersCount(charactersCount, fieldName));

        var response = await RequestHelper.SendRequestAsync(requestType, url, content);

        ScenarioContext.Add(TestConstants.Response, response);
    }

    [When(@"the user sends the (PUT|) request for Manufacturer with ""([^""]*)"" as the (Name|Website|) property value")]
    public async Task WhenTheUserSendsThePUTRequestForManufacturerWithAsTheNameProperty(HttpMethod requestType, string value, string propertyName)
    {
        var manufacturerId = this.GetIdOfFirstManufacturer();

        var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.ManufacturersUrl + "/" + manufacturerId;

        value = value == "empty string" ? string.Empty : null!;

        var content = _formatter.CreateStringContent(this.CreateRequestBodyByPropertyAndValue(value, propertyName));

        var response = await RequestHelper.SendRequestAsync(requestType, url, content);

        ScenarioContext.Add(TestConstants.Response, response);
    }

    [Then(@"the following Manufacturer is updated in the database")]
    public async Task ThenTheManufacturerIsUpdatedInTheDatabase(Table expectedManufacturerDataTable)
    {
        var manufacturerInDatabase =
            await ManufacturerConfigurator.GetByIdAsync(
                ScenarioContext.Get<Guid>(TestConstants.UpdatedManufacturerIdKey));

        var expectedData = expectedManufacturerDataTable.CreateInstance<ManufacturerModel>();

        expectedData.Should().BeEquivalentTo(manufacturerInDatabase, options => options
                .Excluding(x => x.CreatedBy)
                .Excluding(x => x.CreatedDate)
                .Excluding(x => x.Id)
                .Excluding(x => x.LastModifiedBy)
                .Excluding(x => x.LastModifiedDate)
                .Excluding(x => x.Products));
    }

    [Then(@"the Website value did not change")]
    public async Task ThenTheWebsiteValueDidNotChange()
    {
        var baseManufacturerId = ScenarioContext.Get<Guid>(TestConstants.UpdatedManufacturerIdKey);
        var baseManufacturer = ScenarioContext.Get<List<Manufacturer>>(TestConstants.CreatedManufacturersKey).Single(m => m.Id == baseManufacturerId);

        var manufacturerInDatabase =
            await ManufacturerConfigurator.GetByIdAsync(baseManufacturerId);

        baseManufacturer.Website.Should().Be(manufacturerInDatabase.Website, "Unexpected value of Website encountered.");
    }

    [Then(@"the LastModifiedDate field in the database has value for the updated manufacturer")]
    public async Task ThenTheLastModifiedDateFieldInTheDatabaseHasValueForUpdatedManufacturer()
    {
        var baseManufacturerId = ScenarioContext.Get<Guid>(TestConstants.UpdatedManufacturerIdKey);
        var manufacturerInDatabase =
            await ManufacturerConfigurator.GetByIdAsync(baseManufacturerId);

        manufacturerInDatabase.LastModifiedDate.Should().NotBeNull();
    }

    private Guid GetIdOfFirstManufacturer()
    {
        var manufacturersList = ScenarioContext.Get<List<Manufacturer>>(TestConstants.CreatedManufacturersKey);
        var manufacturerIds = new List<Guid>();

        foreach (var manufacturer in manufacturersList)
        {
            manufacturerIds.Add(manufacturer.Id);
        }

        var firstGuid = manufacturerIds.First();
        ScenarioContext.Add(TestConstants.UpdatedManufacturerIdKey, firstGuid);

        return firstGuid;
    }

    private UpdateManufacturerModel CreateRequestBodyWithIncorrectCharactersCount(int charactersNumber, string fieldName)
    {
        return fieldName switch
        {
            "Website" => new UpdateManufacturerModel()
            {
                Name = "TestManufacturer",
                Website = "http://" + new string('a', charactersNumber) + ".com"
            },
            "Name" => new UpdateManufacturerModel
            {
                Name = new string('a', charactersNumber),
                Website = "www.example.org"
            },
            _ => throw new ArgumentException(
                $"Provided field name '{fieldName}' does not exist for Update Manufacturer model.")
        };
    }

    private UpdateManufacturerModel CreateRequestBodyByPropertyAndValue(string value, string fieldName)
    {
        return fieldName switch
        {
            "Website" => new UpdateManufacturerModel()
            {
                Name = "TestManufacturerUpdated",
                Website = value
            },
            "Name" => new UpdateManufacturerModel
            {
                Name = value,
                Website = "www.example.org"
            },
            _ => throw new ArgumentException(
                $"Provided field name '{fieldName}' does not exist for Update Manufacturer model.")
        };
    }
}