using Castle.Components.DictionaryAdapter;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using IS.ScaleModelsShop.Application.Middleware;
using MediatR;
using Moq;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace IS.ScaleModelsShop.Application.UnitTests.ValidationBehaviour;

[TestFixture]
public class ValidationForEmptyResponseBehaviourTests
{
    private IEnumerable<IValidator<IRequest>> fakeValidators;
    private ValidationForEmptyResponseBehaviour<IRequest, object> validationBehavior;
    private Mock<InlineValidator<IRequest>> mockValidator;

    [Test]
    public void Constructor_WhenCalledWithNoValidators_ShouldThrowArgumentNullException()
    {
        Func<ValidationBehavior<IRequest<object>, object>> result = () => new ValidationBehavior<IRequest<object>, object>(null);

        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public async Task Handle_WhenCalledWithNullNext_ShouldThrowArgumentNullException()
    {
        this.mockValidator = new Mock<InlineValidator<IRequest>>();

        this.mockValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<IRequest>>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult(new List<ValidationFailure>
            { new ValidationFailure("test", "test") }));

        this.fakeValidators = new List<IValidator<IRequest>> { this.mockValidator.Object };

        this.validationBehavior = new ValidationForEmptyResponseBehaviour<IRequest, object>(this.fakeValidators);


        Func<Task<object>> result = async () => await this.validationBehavior.Handle(Mock.Of<IRequest>(), null, CancellationToken.None);

        await result.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task Handle_WhenCalledWithValidationFailure_ShouldThrowValidationException()
    {
        var validationException = new ValidationException("ValidationException");

        this.mockValidator = new Mock<InlineValidator<IRequest>>();

        this.mockValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<IRequest>>(), It.IsAny<CancellationToken>())).ThrowsAsync(validationException);

        this.fakeValidators = new List<IValidator<IRequest>> { this.mockValidator.Object };

        this.validationBehavior = new ValidationForEmptyResponseBehaviour<IRequest, object>(this.fakeValidators);

        Func<Task<object>> result = async () => await this.validationBehavior.Handle(Mock.Of<IRequest>(), () => Task.FromResult(new object()), CancellationToken.None);
        await result.Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task Handle_WhenCalled_ShouldNotThrowException()
    {
        this.mockValidator = new Mock<InlineValidator<IRequest>>();
        this.mockValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<IRequest>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

        this.fakeValidators = new List<IValidator<IRequest>> { this.mockValidator.Object };
        this.validationBehavior = new ValidationForEmptyResponseBehaviour<IRequest, object>(this.fakeValidators);

        Func<Task<object>> result = async () => await this.validationBehavior.Handle(Mock.Of<IRequest>(), () => Task.FromResult(new object()), CancellationToken.None);

        await result.Should().NotThrowAsync();
    }

    [Test]
    public async Task Handle_WhenCalled_ShouldThrowException()
    {
        this.mockValidator = new Mock<InlineValidator<IRequest>>();
        this.mockValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<IRequest>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult { Errors = new EditableList<ValidationFailure> { new ValidationFailure() } });

        this.fakeValidators = new List<IValidator<IRequest>> { this.mockValidator.Object };
        this.validationBehavior = new ValidationForEmptyResponseBehaviour<IRequest, object>(this.fakeValidators);

        Func<Task<object>> result = async () => await this.validationBehavior.Handle(Mock.Of<IRequest>(), () => Task.FromResult(new object()), CancellationToken.None);

        await result.Should().ThrowAsync<ValidationException>();
    }
}