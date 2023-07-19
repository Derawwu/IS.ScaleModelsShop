using IS.ScaleModelsShop.API.AcceptanceTests.Constants;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.FilesProvider;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Manufacturers;

public class ManufacturerConfigurator : IManufacturerConfigurator
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IFileProviderService _fileProviderService;
    private readonly ScenarioContext _scenarioContext;

    private bool disposed = false;

    public ManufacturerConfigurator(IManufacturerRepository manufacturerRepository, IFileProviderService fileProviderService, ScenarioContext scenarioContext)
    {
        _manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
        _fileProviderService = fileProviderService ?? throw new ArgumentNullException(nameof(fileProviderService));
        _scenarioContext = scenarioContext;
    }

    public async Task<Manufacturer> CreateAsync(Manufacturer manufacturer)
    {
        return await _manufacturerRepository.AddAsync(manufacturer);
    }

    public async Task<IEnumerable<Manufacturer>> GetAllAsync()
    {
        return await _manufacturerRepository.GetAllAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        await _manufacturerRepository.DeleteAsync(id);
    }

    public async Task RemoveAllAsync()
    {
        await _manufacturerRepository.RemoveAllAsync();
    }

    public async Task<Manufacturer> GetByIdAsync(Guid id)
    {
        return await _manufacturerRepository.GetByIdAsync(id, true);
    }

    public async Task SetupDataBaseAsync()
    {

        var manufacturersString = _fileProviderService.GetJsonStringFromFile(MockConstants.ManufacturersListOfContent);

        var manufacturersListFromJsonFile = JsonConvert.DeserializeObject<IList<Manufacturer>>(manufacturersString)
                                         ?? throw new JsonReaderException(nameof(CreateManufacturerCommand));
        var manufacturersList = new List<Manufacturer>();

        foreach (var manufacturer in manufacturersListFromJsonFile)
        {
            await _manufacturerRepository.AddAsync(manufacturer);
        }

        var manufacturers = await _manufacturerRepository.GetAllAsync();

        manufacturersList.AddRange(manufacturers.ToList());
        _scenarioContext.Add(TestConstants.CreatedManufacturersKey, manufacturersList);
    }
}