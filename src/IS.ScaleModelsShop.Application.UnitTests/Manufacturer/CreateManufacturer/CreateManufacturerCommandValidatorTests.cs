﻿using System.Linq.Expressions;
using FluentAssertions;
using FluentValidation.TestHelper;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.UpdateManufacturer;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;

namespace IS.ScaleModelsShop.Application.UnitTests.Manufacturer.CreateManufacturer;

public class CreateManufacturerCommandValidatorTests
{
    private CreateManufacturerCommand _fakeCreateManufacturerCommand;
    private Mock<IManufacturerRepository> _manufacturerRepositoryMock;
    private CreateManufacturerCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();

        _validator = new CreateManufacturerCommandValidator(_manufacturerRepositoryMock.Object);

        _fakeCreateManufacturerCommand = new CreateManufacturerCommand
        {
            Name = nameof(CreateManufacturerCommand),
            Website = "www.example.org"
        };
    }

    [Test]
    public void Constructor_WhenCalledWithoutCategoryRepository_ShouldThrowArgumentNullException()
    {
        Action result = () => new CreateManufacturerCommandValidator(null);

        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public async Task Validator_WhenNameIsNull_ShouldHaveError()
    {
        var propertyName = nameof(CreateManufacturerCommand.Name);
        _fakeCreateManufacturerCommand.Name = null;

        var result = await _validator.TestValidateAsync(_fakeCreateManufacturerCommand);
        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"'{propertyName}' must not be empty.");
    }

    [Test]
    public async Task Validator_WhenNameIsEmpty_ShouldHaveError()
    {
        var propertyName = nameof(CreateManufacturerCommand.Name);
        _fakeCreateManufacturerCommand.Name = string.Empty;

        var result = await _validator.TestValidateAsync(_fakeCreateManufacturerCommand);
        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"'{propertyName}' must not be empty.");
    }

    [Test]
    public async Task Validator_WhenNameIsTooShort_ShouldHaveError()
    {
        var propertyName = nameof(CreateManufacturerCommand.Name);
        _fakeCreateManufacturerCommand.Name = new string('a', 2);

        var result = await _validator.TestValidateAsync(_fakeCreateManufacturerCommand);
        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"'{propertyName}' must be between 3 and 50 characters. You entered 2 characters.");
    }

    [Test]
    public async Task Validator_WhenNameIsTooLong_ShouldHaveError()
    {
        var propertyName = nameof(CreateManufacturerCommand.Name);
        _fakeCreateManufacturerCommand.Name = new string('a', 51);

        var result = await _validator.TestValidateAsync(_fakeCreateManufacturerCommand);
        result.ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"'{propertyName}' must be between 3 and 50 characters. You entered 51 characters.");
    }

    [Test]
    public async Task Validator_WhenDuplicatedNameProvided_ShouldHaveError()
    {
        var propertyName = nameof(CreateManufacturerCommand.Name);
        _manufacturerRepositoryMock.Setup(x =>
            x.AnyAsync(It.Is<Expression<Func<Domain.Entities.Manufacturer, bool>>>(s =>
                s.Body.ToString().Contains(nameof(Domain.Entities.Manufacturer.Name))))).ReturnsAsync(true);

        var result = await _validator.TestValidateAsync(_fakeCreateManufacturerCommand);

        result
            .ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"Manufacturer with the same '{propertyName}' already exists.");
    }

    [Test]
    public async Task Validator_WhenCalledWithWrongWebsiteUrlFormat_ShouldHaveError()
    {
        var propertyName = nameof(CreateManufacturerCommand.Website);
        _fakeCreateManufacturerCommand.Website = "TestWebsite";

        var result = await _validator.TestValidateAsync(_fakeCreateManufacturerCommand);

        result
            .ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage(
                $"'{propertyName}' is not in the correct format. Expected format - \"http(s)://\" or \"www.\" following with the site name and domain name (e.g., www.example.org)");
    }

    [Test]
    public async Task Validator_WhenCalledWithInappropriateWebsiteUrlDomain_ShouldHaveError()
    {
        var propertyName = nameof(CreateManufacturerCommand.Website);
        _fakeCreateManufacturerCommand.Website = "www.example.ru";

        var result = await _validator.TestValidateAsync(_fakeCreateManufacturerCommand);

        result
            .ShouldHaveValidationErrorFor(propertyName)
            .WithErrorMessage($"Provided '{propertyName}' has unacceptable domain name( e.g., \".ru\" or \".su\")");
    }
}