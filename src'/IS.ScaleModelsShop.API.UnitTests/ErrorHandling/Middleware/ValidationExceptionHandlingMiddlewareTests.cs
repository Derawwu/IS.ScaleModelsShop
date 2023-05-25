using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using IS.ScaleModelsShop.API.Common;
using IS.ScaleModelsShop.API.Middleware;
using IS.ScaleModelsShop.API.Responses;
using IS.ScaleModelsShop.API.UnitTests.TestEntities.Mock.Http;
using Microsoft.AspNetCore.Http;
using Moq;

namespace IS.ScaleModelsShop.API.UnitTests.ErrorHandling.Middleware
{
    [TestFixture]
    public class ValidationExceptionHandlingMiddlewareTests
    {
        private const string Path = "/TestPath";
        private const string TraceId = "TestTraceId";

        private Mock<HttpContext> mockHttpContext;
        private Mock<HttpResponse> mockHttpResponse;
        private Mock<RequestDelegate> mockRequestDelegate;
        private Mock<IResponseHandler> mockResponseHandler;
        private HttpRequest fakeHttpRequest;
        private ValidationExceptionMiddleware validationErrorsHandlingMiddleware;

        [SetUp]
        public void Setup()
        {
            mockHttpContext = new Mock<HttpContext>();
            mockRequestDelegate = new Mock<RequestDelegate>();
            mockResponseHandler = new Mock<IResponseHandler>();

            fakeHttpRequest = new FakeHttpRequest
            {
                Path = new PathString(Path)
            };

            mockHttpResponse = new Mock<HttpResponse>();

            mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(x => x.TraceIdentifier).Returns(TraceId);
            mockHttpContext.SetupGet(x => x.Request).Returns(fakeHttpRequest);
            mockHttpContext.SetupGet(x => x.Response).Returns(mockHttpResponse.Object);

            validationErrorsHandlingMiddleware = new ValidationExceptionMiddleware(mockRequestDelegate.Object, mockResponseHandler.Object);
        }

        #region Constructor

        [Test]
        public void Constructor_WhenCalledWithValidArguments_InstanceShouldBeCreated()
        {
            validationErrorsHandlingMiddleware.Should().NotBeNull();
        }

        [Test]
        public void Constructor_WhenInstanceCreatedWithNullRequestDelegate_ExceptionShouldBeThrown()
        {
            Func<ValidationExceptionMiddleware> result = () => new ValidationExceptionMiddleware(null, mockResponseHandler.Object);
            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Constructor_WhenInstanceCreatedWithNullResponseHandler_ExceptionShouldBeThrown()
        {
            Func<ValidationExceptionMiddleware> result = () => new ValidationExceptionMiddleware(mockRequestDelegate.Object, null);
            result.Should().Throw<ArgumentNullException>();
        }

        #endregion

        #region Invoke

        [Test]
        public async Task Invoke_WhenContextNull_NextRequestDelegateInvoked()
        {
            Func<Task> result = async () => await validationErrorsHandlingMiddleware.Invoke(null);
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Test]
        public async Task Invoke_CorrectData_NextRequestDelegateInvoked()
        {
            mockRequestDelegate.Setup(next => next.Invoke(mockHttpContext.Object));

            await validationErrorsHandlingMiddleware.Invoke(mockHttpContext.Object);

            mockRequestDelegate.Verify(next => next.Invoke(mockHttpContext.Object), Times.Once);
        }

        [Test]
        public async Task Invoke_ValidationExceptionThrown_ResponseHandlerShouldBeCalled()
        {
            var errorMessage = "TestErrorMessage";
            var validationException = new ValidationException(errorMessage, new List<ValidationFailure> { new ValidationFailure("test property", "test error") });

            mockHttpResponse.SetupSet(x => x.StatusCode = It.IsAny<int>()).Callback<int>(actualStatusCode => actualStatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity));
            mockRequestDelegate.Setup(next => next.Invoke(mockHttpContext.Object)).ThrowsAsync(validationException);

            await validationErrorsHandlingMiddleware.Invoke(mockHttpContext.Object);

            mockResponseHandler.Verify(x => x.HandleResponseAsync(It.IsAny<HttpContext>(), It.IsAny<ValidationException>(), It.IsAny<ExceptionResponse>()), Times.Once);
        }

        [Test]
        public async Task Invoke_ValidationExceptionThrownWithErrorCode_ResponseHandlerShouldBeCalled()
        {
            var errorMessage = "TestErrorMessage";
            var validationException = new ValidationException(errorMessage, new List<ValidationFailure> { new ValidationFailure("test property", "test error") { ErrorCode = "400" } });

            mockHttpResponse.SetupSet(x => x.StatusCode = It.IsAny<int>()).Callback<int>(actualStatusCode => actualStatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity));
            mockRequestDelegate.Setup(next => next.Invoke(mockHttpContext.Object)).ThrowsAsync(validationException);

            await validationErrorsHandlingMiddleware.Invoke(mockHttpContext.Object);

            mockResponseHandler.Verify(x => x.HandleResponseAsync(It.IsAny<HttpContext>(), It.IsAny<ValidationException>(), It.IsAny<ExceptionResponse>()), Times.Once);
        }

        #endregion
    }
}