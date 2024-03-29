﻿using AutoMapper;
using FluentAssertions;
using IS.ScaleModelsShop.Application.Features.Products.Commands.CreateProduct;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;
using System.Linq.Expressions;

namespace IS.ScaleModelsShop.Application.UnitTests.Product.CreateProduct;

public class CreateProductCommandHandlerTest
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private CreateProductCommand _fakeProduct;
    private CreateProductCommandHandler _handler;
    private Mock<IManufacturerRepository> _manufacturerRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private Mock<IProductRepository> _productRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();

        _categoryRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Category, bool>>>()))
            .ReturnsAsync(true);
        _manufacturerRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Manufacturer, bool>>>()))
            .ReturnsAsync(true);

        _mapperMock = new Mock<IMapper>();

        _handler = new CreateProductCommandHandler(_mapperMock.Object, _productRepositoryMock.Object,
            _categoryRepositoryMock.Object);

        _fakeProduct = new CreateProductCommand
        {
            CategoryId = Guid.Empty,
            Name = nameof(CreateProductCommand.Name),
            ManufacturerId = Guid.Empty,
            Description = nameof(CreateProductCommand.Description),
            Price = decimal.One
        };
    }

    [Test]
    public void Constructor_WhenCalledWithNoProductRepository_ShouldThrowNewArgumentNullException()
    {
        Action result = () => new CreateProductCommandHandler(_mapperMock.Object, null, _categoryRepositoryMock.Object);
        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenCalledWithNoCategoryRepository_ShouldThrowNewArgumentNullException()
    {
        Action result = () => new CreateProductCommandHandler(_mapperMock.Object, _productRepositoryMock.Object, null);
        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenCalledWithNoMapper_ShouldThrowNewArgumentNullException()
    {
        Action result = () =>
            new CreateProductCommandHandler(null, _productRepositoryMock.Object, _categoryRepositoryMock.Object);
        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public async Task Handler_WhenCalledCreateProduct_ShouldCreateProduct()
    {
        _mapperMock.Setup(m => m.Map<Domain.Entities.Product>(It.IsAny<CreateProductCommand>()))
            .Returns(new Domain.Entities.Product());

        await _handler.Handle(_fakeProduct, CancellationToken.None);

        _categoryRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Once);
        _productRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Product>()), Times.Once);
    }

    [Test]
    public async Task Handler_WhenCalledWithNullRequest_ShouldThrowNewArgumentNullException()
    {
        Func<Task> result = async () => await _handler.Handle(null, CancellationToken.None);

        await result.Should().ThrowAsync<ArgumentNullException>();
    }
}