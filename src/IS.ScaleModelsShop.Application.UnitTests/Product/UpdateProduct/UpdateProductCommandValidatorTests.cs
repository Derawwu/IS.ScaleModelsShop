using AutoMapper;
using IS.ScaleModelsShop.Application.Features.Products.Commands.UpdateProduct;
using IS.ScaleModelsShop.Application.Profiles;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;
using System.Linq.Expressions;
using FluentAssertions;
using FluentValidation.TestHelper;
using IS.ScaleModelsShop.Application.Features.Products.Commands.CreateProduct;

namespace IS.ScaleModelsShop.Application.UnitTests.Product.UpdateProduct;

public class UpdateProductCommandValidatorTests
{
    private Mock<IProductRepository> _productRepositoryMock;
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private Mock<IManufacturerRepository> _manufacturerRepositoryMock;
    private IMapper _mapper;
    private UpdateProductCommandValidator _validator;
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
        _productRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Product, bool>>>()))
            .ReturnsAsync(true);

        var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MapperProfile>(); });

        _mapper = configurationProvider.CreateMapper();

        _fakeProduct = new UpdateProductCommand
        {
            CategoryId = Guid.Empty,
            Name = nameof(UpdateProductCommand.Name),
            ManufacturerId = Guid.Empty,
            Description = nameof(UpdateProductCommand.Description),
            Price = decimal.One
        };

        _validator = new UpdateProductCommandValidator(_productRepositoryMock.Object, _categoryRepositoryMock.Object,
            _manufacturerRepositoryMock.Object);
    }

    [Test]
    public void Constructor_WhenCalledWithoutCategoryRepository_ShouldThrowArgumentNullException()
    {
        Action result = () =>
            new UpdateProductCommandValidator(_productRepositoryMock.Object, null,
                _manufacturerRepositoryMock.Object);

        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenCalledWithoutManufacturerRepository_ShouldThrowArgumentNullException()
    {
        Action result = () =>
            new UpdateProductCommandValidator(_productRepositoryMock.Object, _categoryRepositoryMock.Object, null);

        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenCalledWithoutProductRepository_ShouldThrowArgumentNullException()
    {
        Action result = () =>
            new UpdateProductCommandValidator(null, _categoryRepositoryMock.Object,
                _manufacturerRepositoryMock.Object);

        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public async Task Validator_WhenIdIsEmptyGuid_ShouldHaveError()
    {
        var propertyName = nameof(UpdateProductCommand.Id);
        _fakeProduct.Id = Guid.Empty;

        var result = await _validator.TestValidateAsync(_fakeProduct);

        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"Guid for '{propertyName}' can not be Empty GUID. Please provide correct GUID.");
    }

    [Test]
    public async Task Validator_WhenNameIsNull_ShouldHaveError()
    {
        var propertyName = nameof(UpdateProductCommand.Name);
        _fakeProduct.Name = null;

        var result = await _validator.TestValidateAsync(_fakeProduct);
        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"'{propertyName}' must not be empty.");
    }

    [Test]
    public async Task Validator_WhenNameIsEmpty_ShouldHaveError()
    {
        var propertyName = nameof(UpdateProductCommand.Name);
        _fakeProduct.Name = string.Empty;

        var result = await _validator.TestValidateAsync(_fakeProduct);
        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"'{propertyName}' must not be empty.");
    }

    [Test]
    public async Task Validator_WhenNameIsTooLong_ShouldHaveError()
    {
        var propertyName = nameof(UpdateProductCommand.Name);
        _fakeProduct.Name = new string('a', 51);

        var result = await _validator.TestValidateAsync(_fakeProduct);
        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage(
                $"The length of '{propertyName}' must be 50 characters or fewer. You entered 51 characters.");
    }

    [Test]
    public async Task Validator_WhenDuplicatedNameProvided_ShouldHaveError()
    {
        var propertyName = nameof(UpdateProductCommand.Name);
        _categoryRepositoryMock.Setup(x =>
            x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Category, bool>>>())).ReturnsAsync(true);

        _manufacturerRepositoryMock.Setup(x =>
            x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Manufacturer, bool>>>())).ReturnsAsync(true);

        _productRepositoryMock.Setup(x =>
            x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Product, bool>>>())).ReturnsAsync(true);


        var result = await _validator.TestValidateAsync(_fakeProduct);

        result
            .ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"Product with the same '{propertyName}' already exists.");
    }

    [Test]
    public async Task Validator_WhenDescriptionIsTooLong_ShouldHaveError()
    {
        var propertyName = nameof(UpdateProductCommand.Description);
        _fakeProduct.Description = new string('a', 501);

        var result = await _validator.TestValidateAsync(_fakeProduct);

        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage(
                $"The length of '{propertyName}' must be 500 characters or fewer. You entered 501 characters.");
    }

    [Test]
    public async Task Validator_WhenPriceIsZero_ShouldHaveError()
    {
        var propertyName = nameof(UpdateProductCommand.Price);
        _fakeProduct.Price = decimal.Zero;

        var result = await _validator.TestValidateAsync(_fakeProduct);

        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage(
                $"Price of the product cannot be equal to '{_fakeProduct.Price}'.Should be greater that zero.");
    }

    [Test]
    public async Task Validator_WhenPriceIsLessThanZero_ShouldHaveError()
    {
        var propertyName = nameof(UpdateProductCommand.Price);
        _fakeProduct.Price = decimal.MinusOne;

        var result = await _validator.TestValidateAsync(_fakeProduct);

        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage(
                $"Price of the product cannot be equal to '{_fakeProduct.Price}'.Should be greater that zero.");
    }

    [Test]
    public async Task Validator_WhenManufacturerIdIsEmptyGUID_ShouldHaveError()
    {
        var propertyName = nameof(CreateProductCommand.ManufacturerId);
        _fakeProduct.ManufacturerId = Guid.Empty;

        var result = await _validator.TestValidateAsync(_fakeProduct);

        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"Guid for '{propertyName}' can not be Empty GUID. Please provide correct GUID.");
    }

    [Test]
    public async Task Validator_WhenManufacturerIdDoesNotExist_ShouldHaveError()
    {
        _manufacturerRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Manufacturer, bool>>>()))
            .ReturnsAsync(false);

        var propertyName = nameof(CreateProductCommand.ManufacturerId);
        _fakeProduct.ManufacturerId = Guid.NewGuid();

        var result = await _validator.TestValidateAsync(_fakeProduct);

        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"Manufacturer with ID '{_fakeProduct.ManufacturerId}' does not exist.");
    }

    [Test]
    public async Task Validator_WhenCategoryIdIsEmptyGUID_ShouldHaveError()
    {
        var propertyName = nameof(CreateProductCommand.CategoryId);
        _fakeProduct.CategoryId = Guid.Empty;

        var result = await _validator.TestValidateAsync(_fakeProduct);

        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"Guid for '{propertyName}' can not be Empty GUID. Please provide correct GUID.");
    }

    [Test]
    public async Task Validator_WhenCategoryIdDoesNotExist_ShouldHaveError()
    {
        _categoryRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Category, bool>>>()))
            .ReturnsAsync(false);

        var propertyName = nameof(CreateProductCommand.CategoryId);
        _fakeProduct.CategoryId = Guid.NewGuid();

        var result = await _validator.TestValidateAsync(_fakeProduct);

        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"Category with ID '{_fakeProduct.CategoryId}' does not exist.");
    }
}