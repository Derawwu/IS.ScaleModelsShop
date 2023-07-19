using BoDi;
using FluentAssertions;
using IS.ScaleModelsShop.API.AcceptanceTests.Communication;
using IS.ScaleModelsShop.API.AcceptanceTests.Constants;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Manufacturers;
using IS.ScaleModelsShop.API.Contracts.Manufacturer;
using IS.ScaleModelsShop.Domain.Entities;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Steps.Manufacturers
{
    [Binding]
    public class GetManufacturersSteps : BaseSteps
    {
        protected GetManufacturersSteps(ScenarioContext scenarioContext,
            IHttpClientService requestHelper,
            IManufacturerConfigurator manufacturerConfigurator,
            IObjectContainer container) : base(scenarioContext, requestHelper, manufacturerConfigurator, container)
        {
        }

        [When(@"the user sends the (GET|) request for Manufacturers")]
        public async Task WhenTheUserSendsTheGETRequestForManufacturers(HttpMethod requestType)
        {
            var url = ScenarioContext.Get<Uri>(TestConstants.BaseClientAddress).AbsoluteUri + PathConstants.ManufacturersUrl;

            var response = await RequestHelper.SendRequestAsync(requestType, url);

            ScenarioContext.Add(TestConstants.Response, response);
        }

        [Then(@"all manufacturers are returned")]
        public async Task ThenAllManufacturersAreReturned()
        {
            var response = await ScenarioContext.Get<HttpResponseMessage>(TestConstants.Response).Content
                .ReadAsStringAsync();

            var actualData = JsonConvert.DeserializeObject<List<ManufacturerModel>>(response);

            var expectedData = ScenarioContext.Get<List<Manufacturer>>(TestConstants.CreatedManufacturersKey);

            actualData.Should().BeEquivalentTo(expectedData, options => options
                .Excluding(x => x.CreatedBy)
                .Excluding(x => x.CreatedDate)
                .Excluding(x => x.LastModifiedBy)
                .Excluding(x => x.LastModifiedDate)
                .Excluding(x => x.Products));
        }

        [Then(@"no manufacturers are returned")]
        public async Task ThenNoManufacturersAreReturned()
        {
            var response = await ScenarioContext.Get<HttpResponseMessage>(TestConstants.Response).Content
                .ReadAsStringAsync();

            var actualData = JsonConvert.DeserializeObject<List<ManufacturerModel>>(response);

            actualData.Should().BeEquivalentTo(Array.Empty<ManufacturerModel>());
        }

    }
}