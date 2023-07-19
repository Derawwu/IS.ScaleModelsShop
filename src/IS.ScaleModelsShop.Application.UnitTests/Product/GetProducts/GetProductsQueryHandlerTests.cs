using AutoMapper;
using FluentAssertions;
using IS.ScaleModelsShop.Application.Features.Products.Queries;
using IS.ScaleModelsShop.Application.Models.Queries;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Common;
using Moq;

namespace IS.ScaleModelsShop.Application.UnitTests.Product.GetProducts;

public class GetProductsQueryHandlerTests
{
    private Mock<IMapper> _mapperMock;
    private Mock<IProductRepository> _productRepositoryMock;
    private GetProductsQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mapperMock = new Mock<IMapper>();
        _productRepositoryMock = new Mock<IProductRepository>();

        _handler = new GetProductsQueryHandler(_mapperMock.Object, _productRepositoryMock.Object);
    }

    [Test]
    public void Constructor_WhenCalledWithNoProductRepository_ShouldThrowNewArgumentNullException()
    {
        Action result = () => new GetProductsQueryHandler(_mapperMock.Object, null);
        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenCalledWithNoMapper_ShouldThrowNewArgumentNullException()
    {
        Action result = () => new GetProductsQueryHandler(null, _productRepositoryMock.Object);
        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public async Task Handle_WhenCalledWithNullRequest_ExceptionShouldBeThrown()
    {
        Func<Task> result = async () => await _handler.Handle(null, CancellationToken.None);

        await result.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task Handle_WhenCalled_GetByQueryAsyncShouldBeCalled()
    {
        _productRepositoryMock.Setup(x =>
            x.GetByQueryAsync(It.IsAny<EntityOptionQuery<Domain.Entities.Product>>(), CancellationToken.None, true))
            .ReturnsAsync(new PaginatedCollection<Domain.Entities.Product>(new List<Domain.Entities.Product> { new() }, 1));

        await _handler.Handle(new GetProductsQuery(), CancellationToken.None);

        _productRepositoryMock.Verify(x => x.GetByQueryAsync(It.IsAny<EntityOptionQuery<Domain.Entities.Product>>(), CancellationToken.None, true), Times.Once);
    }
}