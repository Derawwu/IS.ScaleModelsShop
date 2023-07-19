using AutoMapper;
using FluentAssertions;
using IS.ScaleModelsShop.Application.Features.Categories.Queries.GetAllCategoriesList;
using IS.ScaleModelsShop.Application.Models.Queries;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;

namespace IS.ScaleModelsShop.Application.UnitTests.Category.GetCategories;

public class GetAllCategoriesQueryTests
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private GetAllCategoriesListQueryHandler _getAllCategoriesListQueryHandler;
    private Mock<IMapper> _mapperMock;

    [SetUp]
    public void Setup()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();

        _mapperMock = new Mock<IMapper>();

        _getAllCategoriesListQueryHandler =
            new GetAllCategoriesListQueryHandler(_mapperMock.Object, _categoryRepositoryMock.Object);
    }

    [Test]
    public void Constructor_WhenCalledWithNoRepository_ShouldThrowNewArgumentNullException()
    {
        Action result = () => new GetAllCategoriesListQueryHandler(_mapperMock.Object, null);
        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenCalledWithNoMapper_ShouldThrowNewArgumentNullException()
    {
        Action result = () => new GetAllCategoriesListQueryHandler(null, _categoryRepositoryMock.Object);
        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public async Task Handle_WhenCalled_GetAllShouldBeCalled()
    {
        _categoryRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Domain.Entities.Category> { new() }.AsEnumerable());

        await _getAllCategoriesListQueryHandler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);

        _categoryRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task Handle_WhenCalledWithNullRequest_ShouldThrowNewArgumentNullException()
    {
        Func<Task> result = async () => await _getAllCategoriesListQueryHandler.Handle(null, CancellationToken.None);

        await result.Should().ThrowAsync<ArgumentNullException>();
    }
}