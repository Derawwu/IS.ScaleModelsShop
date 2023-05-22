using AutoMapper;
using FluentAssertions;
using IS.ScaleModelsShop.Application.Features.Categories.Queries.GetAllCategoriesList;
using IS.ScaleModelsShop.Application.Profiles;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;

namespace IS.ScaleModelsShop.Application.UnitTests.Category.GetCategories;

public class GetAllCategoriesQueryTests
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private GetAllCategoriesListQueryHandler _getAllCategoriesListQueryHandler;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();

        var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MapperProfile>(); });

        _mapper = configurationProvider.CreateMapper();

        _getAllCategoriesListQueryHandler =
            new GetAllCategoriesListQueryHandler(_mapper, _categoryRepositoryMock.Object);
    }

    [Test]
    public void Constructor_WhenCalledWithNoRepository_ShouldThrowNewArgumentNullException()
    {
        Action result = () => new GetAllCategoriesListQueryHandler(_mapper, null);
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

        await _getAllCategoriesListQueryHandler.Handle(new GetAllCategoriesListQuery(), CancellationToken.None);

        _categoryRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task Handle_WhenCalledWithNullRequest_ShouldThrowNewArgumentNullException()
    {
        Func<Task> result = async () => await _getAllCategoriesListQueryHandler.Handle(null, CancellationToken.None);

        await result.Should().ThrowAsync<ArgumentNullException>();
    }
}