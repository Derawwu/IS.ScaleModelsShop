using IS.ScaleModelsShop.API.AcceptanceTests.Constants;
using IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.FilesProvider;
using IS.ScaleModelsShop.Application.Features.Categories.Commands.CreateCategory;
using IS.ScaleModelsShop.Application.Features.Products.Commands.CreateProduct;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Entities;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.Products
{
    public class ProductConfigurator : IProductConfigurator
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileProviderService _fileProviderService;
        private readonly ScenarioContext _scenarioContext;

        private bool disposed = false;

        public ProductConfigurator(IProductRepository productRepository, ScenarioContext scenarioContext, IFileProviderService fileProviderService)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
            _fileProviderService = fileProviderService ?? throw new ArgumentNullException(nameof(fileProviderService));
        }

        public async Task<Product> CreateAsync(Product product)
        {
            return await _productRepository.AddAsync(product);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            await _productRepository.DeleteAsync(id);
        }

        public async Task RemoveAllAsync()
        {
            await _productRepository.RemoveAllAsync();
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await _productRepository.GetByIdAsync(id, true);
        }

        public async Task SetupDataBaseAsync()
        {
            var productsString = _fileProviderService.GetJsonStringFromFile(MockConstants.ProductsListOfContent);

            var productsListFromJsonFile = JsonConvert.DeserializeObject<IList<Product>>(productsString)
                                             ?? throw new JsonReaderException(nameof(CreateProductCommand));
            var productsList = new List<Product>();

            foreach (var product in productsListFromJsonFile)
            {
                await _productRepository.AddAsync(product);
            }

            var products = await _productRepository.GetAllAsync();

            productsList.AddRange(products.ToList());
            _scenarioContext.Add(TestConstants.CreatedProductsKey, productsList);
        }
    }
}
