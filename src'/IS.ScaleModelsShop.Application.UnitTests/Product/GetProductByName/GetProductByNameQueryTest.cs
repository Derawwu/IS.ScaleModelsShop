using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using IS.ScaleModelsShop.Application.Exceptions;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductByName;
using IS.ScaleModelsShop.Application.Profiles;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;

namespace IS.ScaleModelsShop.Application.UnitTests.Product.GetProductByName;

public class GetProductByNameQueryTest
{
    private GetProductByNameQueryHandler _handler;
    private IMapper _mapper;
    private Mock<IProductRepository> _productRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _productRepositoryMock = new Mock<IProductRepository>();

        var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MapperProfile>(); });

        _mapper = configurationProvider.CreateMapper();

        _handler = new GetProductByNameQueryHandler(_mapper, _productRepositoryMock.Object);
    }

    [Test]
    public void Constructor_WhenCalledWithNoRepository_ShouldThrowNewArgumentNullException()
    {
        Action result = () => new GetProductByNameQueryHandler(_mapper, null);

        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenCalledWithNoMapper_ShouldThrowNewArgumentNullException()
    {
        Action result = () => new GetProductByNameQueryHandler(null, _productRepositoryMock.Object);

        result.Should().Throw<ArgumentNullException>();
    }


    [Test]
    public async Task Handle_WhenCalled_GetEntityByNameShouldBeCalled()
    {
        _productRepositoryMock.Setup(x =>
                x.GetEntityByNameAsync(It.IsAny<Expression<Func<Domain.Entities.Product, bool>>>()))
            .ReturnsAsync(new Domain.Entities.Product());

        await _handler.Handle(new GetProductByNameQuery(), CancellationToken.None);

        _productRepositoryMock.Verify(
            x => x.GetEntityByNameAsync(It.IsAny<Expression<Func<Domain.Entities.Product, bool>>>()), Times.Once);
    }

    [Test]
    public async Task Handle_WhenCalledWithNullRequest_ShouldThrowNewArgumentNullException()
    {
        Func<Task> result = async () => await _handler.Handle(null, CancellationToken.None);

        await result.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task Handle_WhenCalledWithNullProduct_ShouldThrowNotFoundException()
    {
        _productRepositoryMock.Setup(x =>
                x.GetEntityByNameAsync(It.IsAny<Expression<Func<Domain.Entities.Product, bool>>>()))
            .ReturnsAsync((Domain.Entities.Product)null);

        Func<Task> result = async () => await _handler.Handle(new GetProductByNameQuery(), CancellationToken.None);

        await result.Should().ThrowAsync<NotFoundException>();
    }
}