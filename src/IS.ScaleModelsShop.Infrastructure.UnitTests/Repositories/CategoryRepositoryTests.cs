using IS.ScaleModelsShop.Domain.Contracts;
using IS.ScaleModelsShop.Infrastructure.Context;
using Moq;
using System.Text.Json;
using FluentAssertions;
using IS.ScaleModelsShop.Domain.Entities;
using IS.ScaleModelsShop.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;

namespace IS.ScaleModelsShop.Infrastructure.UnitTests.Repositories;

public class CategoryRepositoryTests
{
    private Mock<IDateTime> _mockDateTime;

    private AppDbContext _testDbContext;
    private CategoryRepository _categoryRepository;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _mockDateTime = new Mock<IDateTime>();
        _mockDateTime.Setup(x => x.UtcNow).Returns(DateTime.Now);

        var options = CreateDbContextOptions();
        _testDbContext = new AppDbContext(options);

        PopulateDatabase(_testDbContext);
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
        _categoryRepository = new CategoryRepository(_testDbContext, _mockDateTime.Object);
    }

    public static DbContextOptions<AppDbContext> CreateDbContextOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("InMemoryDatabase")
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
    }

    [Test]
    public async Task GetAllAsync_WhenCalled_ShouldReturnAllEntities()
    {
        var expectedCategories = await _testDbContext.Categories.ToListAsync();

        var result = await _categoryRepository.GetAllAsync();

        result.Should()
            .NotBeNullOrEmpty()
            .And.BeOfType<List<Category>>()
            .And.BeEquivalentTo(expectedCategories);
    }

    [Test]
    public async Task FilterAsync_WhenCalled_ShouldReturnMatchingEntity()
    {
        var result = await _categoryRepository.FilterAsync(x => x.Id == new Guid("00000000-0000-0000-0000-000000000001"), y => y);

        result.Should()
            .NotBeNull()
            .And.BeOfType<List<Category>>()
            .And.HaveCount(1);
    }

    [Test]
    public async Task GetByIdAsync_WhenCalled_ReturnsCategoryIEnumerable()
    {
        var result = await _categoryRepository.GetByIdAsync(new Guid("00000000-0000-0000-0000-000000000001"));

        result.Should()
            .NotBeNull()
            .And.BeOfType<Category>();
    }

    [Test]
    public async Task GetByIdAsync_WhenCalledWIthNonExistentCategoryId_ReturnsNull()
    {
        var result = await _categoryRepository.GetByIdAsync(Guid.NewGuid());

        result.Should()
            .BeNull();
    }

    [Test]
    public async Task AddAsync_WithCorrectData_ReturnsCorrectData()
    {
        var category = new Category
        {
            Name = "Another client",
            CreatedDate = _mockDateTime.Object.UtcNow,
            LastModifiedDate = _mockDateTime.Object.UtcNow
        };

        var addedCategory = await _categoryRepository.AddAsync(category);

        addedCategory.Name.Should().Be(category.Name);
        addedCategory.CreatedDate.Should().Be(category.CreatedDate);
        _testDbContext.Categories.Remove(category);
        await _testDbContext.SaveChangesAsync();
    }

    [Test]
    public async Task UpdateAsync_WhenCalled_ShouldUpdateClient()
    {
        var categoryId = new Guid("00000000-0000-0000-0000-000000000001");
        var existingCategory = await _testDbContext.Categories.Where(s => s.Id == categoryId).SingleAsync();
        _testDbContext.Entry(existingCategory).State = EntityState.Detached;

        var category = new Category
        {
            Id = categoryId
        };

        category.Should().NotBeEquivalentTo(existingCategory);

        await _categoryRepository.UpdateAsync(category);

        var updatedCategory = await _testDbContext.Categories.Where(s => s.Id == categoryId).SingleAsync();

        updatedCategory.Should()
            .NotBeNull().And
            .NotBeEquivalentTo(existingCategory).And
            .BeEquivalentTo(category);

        var inMemoryClient = _testDbContext.Categories.First();
        inMemoryClient.Should().BeEquivalentTo(updatedCategory);
    }

    [Test]
    public async Task DeleteAsync_WhenCalled_ShouldDeleteEntity()
    {
        var categoryId = new Guid("00000000-0000-0000-0000-000000000001");
        var existingCategory = await _testDbContext.Categories.Where(s => s.Id == categoryId).SingleAsync();
        _testDbContext.Entry(existingCategory).State = EntityState.Detached;

        await _categoryRepository.DeleteAsync(categoryId);

        var result = await _testDbContext.Categories.SingleOrDefaultAsync(c => c.Id == categoryId);
        result.Should().BeNull();

        await _testDbContext.Categories.AddAsync(existingCategory);
        await _testDbContext.SaveChangesAsync();
    }

    [Test]
    public async Task AnyAsync_WhenCalledWithExistingCategoryId_ShouldReturnTrue()
    {
        var categoryId = new Guid("00000000-0000-0000-0000-000000000001");

        var result = await _categoryRepository.AnyAsync(c => c.Id == categoryId);

        result.Should().BeTrue();
    }

    [Test]
    public async Task AnyAsync_WhenCalledWithNonExistingCategoryId_ShouldReturnFalse()
    {
        var categoryId = Guid.NewGuid();

        var result = await _categoryRepository.AnyAsync(c => c.Id == categoryId);

        result.Should().BeFalse();
    }

    [Test]
    public async Task GetEntityByNameAsync_WhenCalledWithExistingCategoryName_ShouldReturnEntity()
    {
        var categoryName = "A category";
        var expectedCategory = await _testDbContext.Categories.FirstAsync(x => x.Name == categoryName);

        var result = await _categoryRepository.GetEntityByNameAsync(x => x.Name == categoryName);

        result.Should()
            .BeOfType<Category>()
            .And.BeEquivalentTo(expectedCategory);
    }

    private void PopulateDatabase(AppDbContext appDbContext)
    {
        IEnumerable<Category> categories = new List<Category>
        {
            new Category()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Name = "A category",
                CreatedDate = _mockDateTime.Object.UtcNow,
                LastModifiedDate = _mockDateTime.Object.UtcNow
            },
            new Category()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000002"),
                Name = "Another category",
                CreatedDate = _mockDateTime.Object.UtcNow,
                LastModifiedDate = _mockDateTime.Object.UtcNow
            },
        };

        appDbContext.Categories.AddRange(categories);
        appDbContext.SaveChanges();
    }
}