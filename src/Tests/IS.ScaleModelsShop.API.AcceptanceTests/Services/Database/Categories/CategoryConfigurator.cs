using IS.ScaleModelsShop.API.AcceptanceTests.Constants;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.FilesProvider;
using IS.ScaleModelsShop.Application.Features.Categories.Commands.CreateCategory;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Categories
{
    public class CategoryConfigurator : ICategoryConfigurator
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileProviderService _fileProviderService;
        private readonly ScenarioContext _scenarioContext;

        private bool disposed = false;

        public CategoryConfigurator(ICategoryRepository categoryRepository, IFileProviderService fileProviderService, ScenarioContext scenarioContext)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _fileProviderService = fileProviderService ?? throw new ArgumentNullException(nameof(fileProviderService));
            _scenarioContext = scenarioContext;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            return await _categoryRepository.AddAsync(category);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            await _categoryRepository.DeleteAsync(id);
        }

        public async Task RemoveAllAsync()
        {
            await _categoryRepository.RemoveAllAsync();
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task SetupDataBaseAsync()
        {
            var categoriesString = _fileProviderService.GetJsonStringFromFile(MockConstants.CategoriesListOfContent);

            var categoriesListFromJsonFile = JsonConvert.DeserializeObject<IList<Category>>(categoriesString)
                                               ?? throw new JsonReaderException(nameof(CreateCategoryCommand));
            var categoriesList = new List<Category>();

            foreach (var category in categoriesListFromJsonFile)
            {
                await _categoryRepository.AddAsync(category);
            }

            var categories = await _categoryRepository.GetAllAsync();

            categoriesList.AddRange(categories.ToList());
            _scenarioContext.Add(TestConstants.CreatedCategoriesKey, categoriesList);
        }
    }
}