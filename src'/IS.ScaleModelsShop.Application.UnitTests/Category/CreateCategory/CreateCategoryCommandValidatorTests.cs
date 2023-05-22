using FluentAssertions;
using FluentValidation.TestHelper;
using IS.ScaleModelsShop.Application.Features.Categories.Commands.CreateCategory;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;
using System.Linq.Expressions;

namespace IS.ScaleModelsShop.Application.UnitTests.Category.CreateCategory
{
    public class CreateCategoryCommandValidatorTests
    {
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private CreateCategoryCommandValidator _createCategoryCommandValidator;
        private CreateCategoryCommand _fakeCreateCategoryCommand;

        [SetUp]
        public void Setup()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();

            _createCategoryCommandValidator = new CreateCategoryCommandValidator(_categoryRepositoryMock.Object);

            _fakeCreateCategoryCommand = new()
            {
                Name = nameof(CreateCategoryCommand)
            };
        }

        [Test]
        public void Constructor_WhenCalledWithoutCategoryRepository_ShouldThrowArgumentNullException()
        {
            Action result = () => new CreateCategoryCommandValidator(null);

            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public async Task Validator_WhenNameIsNull_ShouldHaveError()
        {
            var propertyName = nameof(CreateCategoryCommand.Name);
            _fakeCreateCategoryCommand.Name = null;

            var result = await _createCategoryCommandValidator.TestValidateAsync(_fakeCreateCategoryCommand);
            result.ShouldHaveValidationErrorFor(propertyName)
                .WithErrorMessage($"'{propertyName}' must not be empty.");
        }

        [Test]
        public async Task Validator_WhenNameIsEmpty_ShouldHaveError()
        {
            var propertyName = nameof(CreateCategoryCommand.Name);
            _fakeCreateCategoryCommand.Name = string.Empty;

            var result = await _createCategoryCommandValidator.TestValidateAsync(_fakeCreateCategoryCommand);
            result.ShouldHaveValidationErrorFor(propertyName)
                .WithErrorMessage($"'{propertyName}' must not be empty.");
        }

        [Test]
        public async Task Validator_WhenNameIsTooLong_ShouldHaveError()
        {
            var propertyName = nameof(CreateCategoryCommand.Name);
            _fakeCreateCategoryCommand.Name = new string('a', 51);

            var result = await _createCategoryCommandValidator.TestValidateAsync(_fakeCreateCategoryCommand);
            result.ShouldHaveValidationErrorFor(propertyName)
                .WithErrorMessage($"The length of '{propertyName}' must be 50 characters or fewer. You entered 51 characters.");
        }

        [Test]
        public async Task Validator_WhenDuplicatedNameProvided_ShouldHaveError()
        {
            var propertyName = nameof(CreateCategoryCommand.Name);
            _categoryRepositoryMock.Setup(x =>
                x.AnyAsync(It.Is<Expression<Func<Domain.Entities.Category, bool>>>(s =>
                    s.Body.ToString().Contains(nameof(Domain.Entities.Category.Name))))).ReturnsAsync(true);

            var result = await _createCategoryCommandValidator.TestValidateAsync(_fakeCreateCategoryCommand);

            result
                .ShouldHaveValidationErrorFor(propertyName)
                .WithErrorMessage($"Category with the same '{propertyName}' already exists.");
        }
    }
}