using FluentAssertions;
using IS.ScaleModelsShop.Domain.Contracts;
using IS.ScaleModelsShop.Domain.Entities;
using IS.ScaleModelsShop.Infrastructure.Context;
using IS.ScaleModelsShop.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace IS.ScaleModelsShop.Infrastructure.UnitTests.Repositories;

public class ManufacturerRepositoryTests
{
    private Mock<IDateTime> _mockDateTime;

    private AppDbContext _testDbContext;
    private ManufacturerRepository _manufacturerRepository;

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
        _manufacturerRepository = new ManufacturerRepository(_testDbContext, _mockDateTime.Object);
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
        var expectedManufacturers = await _testDbContext.Manufacturers.ToListAsync();

        var result = await _manufacturerRepository.GetAllAsync();

        result.Should()
            .NotBeNullOrEmpty()
            .And.BeOfType<List<Manufacturer>>()
            .And.BeEquivalentTo(expectedManufacturers);
    }

    [Test]
    public async Task FilterAsync_WhenCalled_ShouldReturnMatchingEntity()
    {
        var result = await _manufacturerRepository.FilterAsync(x => x.Id == new Guid("00000000-0000-0000-0000-000000000001"), y => y);

        result.Should()
            .NotBeNull()
            .And.BeOfType<List<Manufacturer>>()
            .And.HaveCount(1);
    }

    [Test]
    public async Task GetByIdAsync_WhenCalled_ReturnsCategoryIEnumerable()
    {
        var result = await _manufacturerRepository.GetByIdAsync(new Guid("00000000-0000-0000-0000-000000000001"));

        result.Should()
            .NotBeNull()
            .And.BeOfType<Manufacturer>();
    }

    [Test]
    public async Task GetByIdAsync_WhenCalledWIthNonExistentCategoryId_ReturnsNull()
    {
        var result = await _manufacturerRepository.GetByIdAsync(Guid.NewGuid());

        result.Should()
            .BeNull();
    }

    [Test]
    public async Task AddAsync_WithCorrectData_ReturnsCorrectData()
    {
        var manufacturer = new Manufacturer
        {
            Name = "Another manufacturer",
            CreatedDate = _mockDateTime.Object.UtcNow,
            LastModifiedDate = _mockDateTime.Object.UtcNow
        };

        var addedCategory = await _manufacturerRepository.AddAsync(manufacturer);

        addedCategory.Name.Should().Be(manufacturer.Name);
        addedCategory.CreatedDate.Should().Be(manufacturer.CreatedDate);
        _testDbContext.Manufacturers.Remove(manufacturer);
        await _testDbContext.SaveChangesAsync();
    }

    [Test]
    public async Task UpdateAsync_WhenCalled_ShouldUpdateClient()
    {
        var manufacturerId = new Guid("00000000-0000-0000-0000-000000000001");
        var existingManufacturer = await _testDbContext.Manufacturers.Where(s => s.Id == manufacturerId).SingleAsync();
        _testDbContext.Entry(existingManufacturer).State = EntityState.Detached;

        var manufacturer = new Manufacturer
        {
            Id = manufacturerId
        };

        manufacturer.Should().NotBeEquivalentTo(existingManufacturer);

        await _manufacturerRepository.UpdateAsync(manufacturer);

        var updatedManufacturer = await _testDbContext.Manufacturers.Where(s => s.Id == manufacturerId).SingleAsync();

        updatedManufacturer.Should()
            .NotBeNull().And
            .NotBeEquivalentTo(existingManufacturer).And
            .BeEquivalentTo(manufacturer);

        var inMemoryManufacturer = _testDbContext.Manufacturers.First();
        inMemoryManufacturer.Should().BeEquivalentTo(updatedManufacturer);
    }

    [Test]
    public async Task DeleteAsync_WhenCalled_ShouldDeleteEntity()
    {
        var manufacturerId = new Guid("00000000-0000-0000-0000-000000000001");
        var existingManufacturer = await _testDbContext.Manufacturers.Where(s => s.Id == manufacturerId).SingleAsync();
        _testDbContext.Entry(existingManufacturer).State = EntityState.Detached;

        await _manufacturerRepository.DeleteAsync(manufacturerId);

        var result = await _testDbContext.Manufacturers.SingleOrDefaultAsync(c => c.Id == manufacturerId);
        result.Should().BeNull();

        await _testDbContext.Manufacturers.AddAsync(existingManufacturer);
        await _testDbContext.SaveChangesAsync();
    }

    [Test]
    public async Task AnyAsync_WhenCalledWithExistingCategoryId_ShouldReturnTrue()
    {
        var manufacturerId = new Guid("00000000-0000-0000-0000-000000000001");

        var result = await _manufacturerRepository.AnyAsync(c => c.Id == manufacturerId);

        result.Should().BeTrue();
    }

    [Test]
    public async Task AnyAsync_WhenCalledWithNonExistingCategoryId_ShouldReturnFalse()
    {
        var manufacturerId = Guid.NewGuid();

        var result = await _manufacturerRepository.AnyAsync(c => c.Id == manufacturerId);

        result.Should().BeFalse();
    }

    [Test]
    public async Task GetEntityByNameAsync_WhenCalledWithExistingCategoryName_ShouldReturnEntity()
    {
        var manufacturerName = "A manufacturer";
        var expectedManufacturer = await _testDbContext.Manufacturers.FirstAsync(x => x.Name == manufacturerName);

        var result = await _manufacturerRepository.GetEntityByNameAsync(x => x.Name == manufacturerName);

        result.Should()
            .BeOfType<Manufacturer>()
            .And.BeEquivalentTo(expectedManufacturer);
    }

    private void PopulateDatabase(AppDbContext appDbContext)
    {
        IEnumerable<Manufacturer> manufacturers = new List<Manufacturer>
        {
            new Manufacturer()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Name = "A manufacturer",
                CreatedDate = _mockDateTime.Object.UtcNow,
                LastModifiedDate = _mockDateTime.Object.UtcNow
            },
            new Manufacturer()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000002"),
                Name = "Another manufacturer",
                CreatedDate = _mockDateTime.Object.UtcNow,
                LastModifiedDate = _mockDateTime.Object.UtcNow
            },
        };

        appDbContext.Manufacturers.AddRange(manufacturers);
        appDbContext.SaveChanges();
    }
}