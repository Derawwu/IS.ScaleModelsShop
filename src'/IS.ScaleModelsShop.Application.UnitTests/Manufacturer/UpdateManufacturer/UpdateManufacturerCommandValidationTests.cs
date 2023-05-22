using System.Linq.Expressions;
using FluentAssertions;
using FluentValidation.TestHelper;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.UpdateManufacturer;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;

namespace IS.ScaleModelsShop.Application.UnitTests.Manufacturer.UpdateManufacturer;

public class UpdateManufacturerCommandValidationTests
{
    private UpdateManufacturerCommand _fakeUpdateManufacturerCommand;
    private Mock<IManufacturerRepository> _manufacturerRepositoryMock;
    private UpdateManufacturerCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();

        _fakeUpdateManufacturerCommand = new UpdateManufacturerCommand
        {
            Name = nameof(Domain.Entities.Manufacturer),
            Id = Guid.Empty,
            Website = "www.example.org"
        };

        _manufacturerRepositoryMock
            .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Manufacturer, bool>>>()))
            .ReturnsAsync(false);
        _manufacturerRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(
            new Domain.Entities.Manufacturer
            {
                Id = _fakeUpdateManufacturerCommand.Id,
                Name = _fakeUpdateManufacturerCommand.Name,
                Website = _fakeUpdateManufacturerCommand.Website
            });

        _validator = new UpdateManufacturerCommandValidator(_manufacturerRepositoryMock.Object);
    }

    [Test]
    public void Constructor_WhenCalledWithoutManufacturerRepository_ShouldThrowArgumentNullException()
    {
        Action result = () => new UpdateManufacturerCommandValidator(null);

        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public async Task Validator_WhenNameIsNull_ShouldHaveError()
    {
        var propertyName = nameof(UpdateManufacturerCommand.Name);
        _fakeUpdateManufacturerCommand.Name = null;

        var result = await _validator.TestValidateAsync(_fakeUpdateManufacturerCommand);
        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"'{propertyName}' must not be empty.");
    }

    [Test]
    public async Task Validator_WhenNameIsEmpty_ShouldHaveError()
    {
        var propertyName = nameof(UpdateManufacturerCommand.Name);
        _fakeUpdateManufacturerCommand.Name = string.Empty;

        var result = await _validator.TestValidateAsync(_fakeUpdateManufacturerCommand);
        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"'{propertyName}' must not be empty.");
    }

    [Test]
    public async Task Validator_WhenNameIsTooShort_ShouldHaveError()
    {
        var propertyName = nameof(UpdateManufacturerCommand.Name);
        _fakeUpdateManufacturerCommand.Name = new string('a', 2);

        var result = await _validator.TestValidateAsync(_fakeUpdateManufacturerCommand);
        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"'{propertyName}' must be between 3 and 50 characters. You entered 2 characters.");
    }

    [Test]
    public async Task Validator_WhenNameIsTooLong_ShouldHaveError()
    {
        var propertyName = nameof(UpdateManufacturerCommand.Name);
        _fakeUpdateManufacturerCommand.Name = new string('a', 51);

        var result = await _validator.TestValidateAsync(_fakeUpdateManufacturerCommand);
        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"'{propertyName}' must be between 3 and 50 characters. You entered 51 characters.");
    }

    [Test]
    public async Task Validator_WhenDuplicatedNameProvided_ShouldHaveError()
    {
        var propertyName = nameof(UpdateManufacturerCommand.Name);
        _manufacturerRepositoryMock.Setup(x =>
            x.AnyAsync(It.Is<Expression<Func<Domain.Entities.Manufacturer, bool>>>(s =>
                s.Body.ToString().Contains(nameof(Domain.Entities.Manufacturer.Name))))).ReturnsAsync(true);

        var result = await _validator.TestValidateAsync(_fakeUpdateManufacturerCommand);

        result
            .ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"Manufacturer with the same '{propertyName}' already exists.");
    }

    [Test]
    public async Task Validator_WhenCalledWithWrongWebsiteUrlFormat_ShouldHaveError()
    {
        var propertyName = nameof(UpdateManufacturerCommand.Website);
        _fakeUpdateManufacturerCommand.Website = "TestWebsite";

        var result = await _validator.TestValidateAsync(_fakeUpdateManufacturerCommand);

        result
            .ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage(
                $"'{propertyName}' is not in the correct format. Expected format - \"http(s)://\" or \"www.\" following with the site name and domain name (e.g., www.example.org)");
    }

    [Test]
    public async Task Validator_WhenCalledWithInappropriateWebsiteUrlDomain_ShouldHaveError()
    {
        var propertyName = nameof(UpdateManufacturerCommand.Website);
        _fakeUpdateManufacturerCommand.Website = "www.example.ru";

        var result = await _validator.TestValidateAsync(_fakeUpdateManufacturerCommand);

        result
            .ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"Provided '{propertyName}' has unacceptable domain name( e.g., \".ru\" or \".su\")");
    }
}