using AutoMapper;
using FluentAssertions;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetAllProductsPaginated;
using IS.ScaleModelsShop.Application.Profiles;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;

namespace IS.ScaleModelsShop.Application.UnitTests.Product.GetAllProductsPaginated;

public class GetAllProductsPaginatedQueryTest
{
    private GetAllProductsPaginatedQueryHandler _handler;
    private IMapper _mapper;
    private Mock<IProductRepository> _productRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _productRepositoryMock = new Mock<IProductRepository>();

        var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MapperProfile>(); });

        _mapper = configurationProvider.CreateMapper();

        _handler = new GetAllProductsPaginatedQueryHandler(_mapper, _productRepositoryMock.Object);
    }

    [Test]
    public void Constructor_WhenCalledWithNoRepository_ShouldThrowNewArgumentNullException()
    {
        Action result = () => new GetAllProductsPaginatedQueryHandler(_mapper, null);

        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenCalledWithNoMapper_ShouldThrowNewArgumentNullException()
    {
        Action result = () => new GetAllProductsPaginatedQueryHandler(null, _productRepositoryMock.Object);

        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public async Task Handle_WhenCalled_GetPaginatedProductsShouldBeCalled()
    {
        _productRepositoryMock.Setup(x => x.GetPaginatedProductsAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new List<Domain.Entities.Product> { new() }.AsEnumerable());

        await _handler.Handle(new GetAllProductsPaginatedQuery(), CancellationToken.None);

        _productRepositoryMock.Verify(x => x.GetPaginatedProductsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Test]
    public async Task Handle_WhenCalledWithNullRequest_ShouldThrowNewArgumentNullException()
    {
        Func<Task> result = async () => await _handler.Handle(null, CancellationToken.None);

        await result.Should().ThrowAsync<ArgumentNullException>();
    }
}