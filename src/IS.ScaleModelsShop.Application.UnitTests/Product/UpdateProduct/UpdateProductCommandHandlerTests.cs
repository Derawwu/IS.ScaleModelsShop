using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using IS.ScaleModelsShop.Application.Exceptions;
using IS.ScaleModelsShop.Application.Features.Products.Commands.CreateProduct;
using IS.ScaleModelsShop.Application.Features.Products.Commands.UpdateProduct;
using IS.ScaleModelsShop.Application.Profiles;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;

namespace IS.ScaleModelsShop.Application.UnitTests.Product.UpdateProduct;

public class UpdateProductCommandHandlerTests
{
    private Mock<IProductRepository> _productRepositoryMock;
    private Mock<IManufacturerRepository> _manufacturerRepositoryMock;
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private UpdateProductCommandHandler _handler;
    private UpdateProductCommand _fakeProduct;

    [SetUp]
    public void Setup()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();

        _categoryRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Category, bool>>>()))
            .ReturnsAsync(true);
        _manufacturerRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Manufacturer, bool>>>()))
            .ReturnsAsync(true);
        _productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
            .ReturnsAsync(new Domain.Entities.Product());

        _mapperMock = new Mock<IMapper>();

        _fakeProduct = new UpdateProductCommand
        {
            CategoryId = Guid.Empty,
            Name = nameof(CreateProductCommand.Name),
            ManufacturerId = Guid.Empty,
            Description = nameof(CreateProductCommand.Description),
            Price = decimal.One
        };

        _handler = new UpdateProductCommandHandler(_productRepositoryMock.Object, _mapperMock.Object);
    }

    [Test]
    public void Constructor_WhenCalledWithNoProductRepository_ShouldThrowNewArgumentNullException()
    {
        Action result = () => new UpdateProductCommandHandler(null, _mapperMock.Object);
        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenCalledWithNoMapper_ShouldThrowNewArgumentNullException()
    {
        Action result = () =>
            new UpdateProductCommandHandler(_productRepositoryMock.Object, null);
        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public async Task Handler_WhenCalledCreateProduct_ShouldCreateProduct()
    {
        await _handler.Handle(_fakeProduct, CancellationToken.None);

        _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Domain.Entities.Product>()), Times.Once);
    }

    [Test]
    public async Task Handler_WhenCalledWIthNullRequest_ShouldThrowNewArgumentNullException()
    {
        Func<Task> result = async () => await _handler.Handle(null, CancellationToken.None);

        await result.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task Handle_WhenCalledWithNullProduct_ShouldThrowNotFoundException()
    {
        _productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
            .ReturnsAsync((Domain.Entities.Product)null);

        Func<Task> result = async () => await _handler.Handle(new UpdateProductCommand(), CancellationToken.None);

        await result.Should().ThrowAsync<EntityNotFoundException>();
    }
}