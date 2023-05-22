using AutoMapper;
using FluentAssertions;
using IS.ScaleModelsShop.Application.Features.Categories.Commands.CreateCategory;
using IS.ScaleModelsShop.Application.Profiles;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;

namespace IS.ScaleModelsShop.Application.UnitTests.Category.CreateCategory;

public class CreateCategoryCommandHandlerTests
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private CreateCategoryCommandHandler _createCategoryCommandHandler;
    private CreateCategoryCommand _fakeCategoryCommand;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();

        var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MapperProfile>(); });

        _mapper = configurationProvider.CreateMapper();

        _fakeCategoryCommand = new CreateCategoryCommand
        {
            Name = nameof(Domain.Entities.Category)
        };

        _createCategoryCommandHandler = new CreateCategoryCommandHandler(_categoryRepositoryMock.Object, _mapper);
    }

    [Test]
    public void Constructor_WhenCalledWithNoRepository_ShouldThrowNewArgumentNullException()
    {
        Action result = () => new CreateCategoryCommandHandler(null, _mapper);
        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenCalledWithNoMapper_ShouldThrowNewArgumentNullException()
    {
        Action result = () => new CreateCategoryCommandHandler(_categoryRepositoryMock.Object, null);
        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public async Task Handler_WhenCalledCreateCategory_ShouldCreateCategory()
    {
        await _createCategoryCommandHandler.Handle(_fakeCategoryCommand, CancellationToken.None);

        _categoryRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Category>()), Times.Once);
    }

    [Test]
    public async Task Handler_WhenCalledWIthNullRequest_ShouldThrowNewArgumentNullException()
    {
        Func<Task> result = async () => await _createCategoryCommandHandler.Handle(null, CancellationToken.None);

        await result.Should().ThrowAsync<ArgumentNullException>();
    }
}