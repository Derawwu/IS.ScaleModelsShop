using FluentAssertions;
using IS.ScaleModelsShop.Domain.Contracts;
using IS.ScaleModelsShop.Domain.Entities;
using IS.ScaleModelsShop.Infrastructure.Context;
using IS.ScaleModelsShop.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using NSubstitute;

namespace IS.ScaleModelsShop.Infrastructure.UnitTests.Repositories;

public class ProductRepositoryTests
{
    private Mock<IDateTime> _mockDateTime;

    private AppDbContext _testDbContext;
    private ProductRepository _productRepository;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _mockDateTime = new Mock<IDateTime>();
        _mockDateTime.Setup(x => x.UtcNow).Returns(DateTime.Now);

        var options = CreateDbContextOptions();
        _testDbContext = new AppDbContext(options);

        PopulateDatabase(_testDbContext);

        _productRepository = new ProductRepository(_testDbContext, _mockDateTime.Object);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _testDbContext.Database.EnsureDeleted();
        _testDbContext.Dispose();
    }

    [SetUp]
    public void Setup()
    {
        _productRepository = new ProductRepository(_testDbContext, _mockDateTime.Object);
    }

    public static DbContextOptions<AppDbContext> CreateDbContextOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("InMemoryDatabase")
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .EnableSensitiveDataLogging()
            .Options;
    }

    [Test]
    public async Task GetAllAsync_WhenCalled_ShouldReturnAllEntities()
    {
        var expectedProduct = await _testDbContext.Products.AsNoTracking().ToListAsync();

        var result = await _productRepository.GetAllAsync();

        result.Should()
            .NotBeNullOrEmpty()
            .And.BeOfType<List<Product>>()
            .And.BeEquivalentTo(expectedProduct);
    }

    [Test]
    public async Task FilterAsync_WhenCalled_ShouldReturnMatchingEntity()
    {
        var result = await _productRepository.FilterAsync(x => x.Id == new Guid("00000000-0000-0000-0000-000000000001"), y => y);

        result.Should()
            .NotBeNull()
            .And.BeOfType<List<Product>>()
            .And.HaveCount(1);
    }

    [Test]
    public async Task GetByIdAsync_WhenCalled_ReturnsCategoryIEnumerable()
    {
        var result = await _productRepository.GetByIdAsync(new Guid("00000000-0000-0000-0000-000000000001"));

        result.Should()
            .NotBeNull()
            .And.BeOfType<Product>();
    }

    [Test]
    public async Task GetByIdAsync_WhenCalledWIthNonExistentCategoryId_ReturnsNull()
    {
        var result = await _productRepository.GetByIdAsync(Guid.NewGuid());

        result.Should()
            .BeNull();
    }

    [Test]
    public async Task AddAsync_WithCorrectData_ReturnsCorrectData()
    {
        var product = new Product
        {
            Name = "Another product",
            Description = "Test Description",
            CreatedDate = _mockDateTime.Object.UtcNow,
            LastModifiedDate = _mockDateTime.Object.UtcNow
        };

        var addedProduct = await _productRepository.AddAsync(product);

        addedProduct.Name.Should().Be(product.Name);
        addedProduct.CreatedDate.Should().Be(product.CreatedDate);
        _testDbContext.Products.Remove(product);
        await _testDbContext.SaveChangesAsync();
    }

    [Test]
    public async Task UpdateAsync_WhenCalled_ShouldUpdateClient()
    {
        var productId = new Guid("00000000-0000-0000-0000-000000000001");
        var existingProduct = await _testDbContext.Products.AsNoTracking().SingleAsync(s => s.Id == productId);
        //_testDbContext.Entry(existingProduct).State = EntityState.Detached;
        _testDbContext.ChangeTracker.Clear();

        var product = new Product
        {
            Id = productId
        };

        product.Should().NotBeEquivalentTo(existingProduct);

        await _productRepository.UpdateAsync(product);

        var updatedManufacturer = await _testDbContext.Products.Where(s => s.Id == productId).SingleAsync();

        updatedManufacturer.Should()
            .NotBeNull().And
            .NotBeEquivalentTo(existingProduct).And
            .BeEquivalentTo(product);

        var inMemoryManufacturer = _testDbContext.Products.First();
        inMemoryManufacturer.Should().BeEquivalentTo(updatedManufacturer);
    }

    [Test]
    public async Task DeleteAsync_WhenCalled_ShouldDeleteEntity()
    {
        var productId = new Guid("00000000-0000-0000-0000-000000000001");
        var existingProduct = await _testDbContext.Products.Where(s => s.Id == productId).SingleAsync();
        _testDbContext.Entry(existingProduct).State = EntityState.Detached;

        await _productRepository.DeleteAsync(productId);

        var result = await _testDbContext.Products.SingleOrDefaultAsync(c => c.Id == productId);
        result.Should().BeNull();

        await _testDbContext.Products.AddAsync(existingProduct);
        await _testDbContext.SaveChangesAsync();
    }

    [Test]
    public async Task AnyAsync_WhenCalledWithExistingCategoryId_ShouldReturnTrue()
    {
        var productId = new Guid("00000000-0000-0000-0000-000000000001");

        var result = await _productRepository.AnyAsync(c => c.Id == productId);

        result.Should().BeTrue();
    }

    [Test]
    public async Task AnyAsync_WhenCalledWithNonExistingCategoryId_ShouldReturnFalse()
    {
        var productId = Guid.NewGuid();

        var result = await _productRepository.AnyAsync(c => c.Id == productId);

        result.Should().BeFalse();
    }

    [Test]
    public async Task GetEntityByNameAsync_WhenCalledWithExistingCategoryName_ShouldReturnEntity()
    {
        var productName = "A product";
        var expectedProduct = await _testDbContext.Products.FirstAsync(x => x.Name == productName);

        var result = await _productRepository.GetEntityByNameAsync(x => x.Name == productName);

        result.Should()
            .BeOfType<Product>()
            .And.BeEquivalentTo(expectedProduct);
    }

    private void PopulateDatabase(AppDbContext appDbContext)
    {
        IEnumerable<Product> products = new List<Product>
        {
            new Product()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Name = "A product",
                Description = "Test Description",
                CreatedDate = _mockDateTime.Object.UtcNow,
                LastModifiedDate = _mockDateTime.Object.UtcNow
            },
            new Product()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000002"),
                Name = "Another product",
                Description = "Test Description",
                CreatedDate = _mockDateTime.Object.UtcNow,
                LastModifiedDate = _mockDateTime.Object.UtcNow
            },
        };

        appDbContext.Products.AddRange(products);

        var category = new Category
        {
            Id = new Guid("00000000-0000-0000-0000-000000000003"),
            Name = "A category"
        };

        appDbContext.Categories.Add(category);

        var productCategory = new List<ProductCategory>
        {
            new ProductCategory
            {
                LinkedCategoryId = new Guid("00000000-0000-0000-0000-000000000003"),
                LinkedProductId = new Guid("00000000-0000-0000-0000-000000000001"),
                Id = Guid.NewGuid()
            },
            new ProductCategory()
            {
                LinkedCategoryId = new Guid("00000000-0000-0000-0000-000000000003"),
                LinkedProductId = new Guid("00000000-0000-0000-0000-000000000002"),
                Id = Guid.NewGuid()
            }
        };

        appDbContext.ProductCategory.AddRange(productCategory);

        appDbContext.SaveChanges();
    }
}