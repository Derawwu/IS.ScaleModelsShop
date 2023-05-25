using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using IS.ScaleModelsShop.API.Common;
using IS.ScaleModelsShop.API.Middleware;
using IS.ScaleModelsShop.API.Resources;
using IS.ScaleModelsShop.API.Responses;
using IS.ScaleModelsShop.API.UnitTests.TestEntities.Mock.Http;
using Microsoft.AspNetCore.Http;
using Moq;

namespace IS.ScaleModelsShop.API.UnitTests.ErrorHandling.Middleware
{
    [TestFixture]
    public class ErrorHandlingMiddlewareTests
    {
        private const string TraceId = "TestTraceId";
        private const string Path = "/TestPath";
        private const string ContentType = "application/problem+json";
        private const string ErrorMessage = "TestErrorMessage";

        private static readonly object[] validationExceptionCases =
            {
                new object[]
                {
                    StatusCodes.Status400BadRequest.ToString(),
                    ErrorTitles.ValidationError,
                    ErrorTypes.BadRequest,
                    StatusCodes.Status400BadRequest
                },
                new object[]
                {
                    StatusCodes.Status404NotFound.ToString(),
                    ErrorTitles.NotFoundError,
                    ErrorTypes.NotFound,
                    StatusCodes.Status404NotFound
                },
                new object[]
                {
                    StatusCodes.Status422UnprocessableEntity.ToString(),
                    ErrorTitles.ValidationError,
                    ErrorTypes.UnprocessableEntity,
                    StatusCodes.Status422UnprocessableEntity
                }
            };

        private Mock<HttpContext> mockHttpContext;
        private Mock<HttpResponse> mockHttpResponse;
        private Mock<RequestDelegate> mockRequestDelegate;
        private Mock<IResponseHandler> mockResponseHandler;

        private HttpRequest fakeHttpRequest;
        private ErrorHandlingMiddleware errorHandlingMiddleware;

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

            errorHandlingMiddleware = new ErrorHandlingMiddleware(mockRequestDelegate.Object, mockResponseHandler.Object);
        }

        #region Constructor

        [Test]
        public void Constructor_WhenCalledWithValidArguments_InstanceShouldBeCreated()
        {
            errorHandlingMiddleware.Should().NotBeNull();
        }

        [Test]
        public void Constructor_WhenInstanceCreatedWithNullRequestDelegate_ExceptionShouldBeThrown()
        {
            Func<ErrorHandlingMiddleware> result = () => new ErrorHandlingMiddleware(null, mockResponseHandler.Object);
            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Constructor_WhenInstanceCreatedWithNullResponseHandler_ExceptionShouldBeThrown()
        {
            Func<ErrorHandlingMiddleware> result = () => new ErrorHandlingMiddleware(mockRequestDelegate.Object, null);
            result.Should().Throw<ArgumentNullException>();
        }

        #endregion

        #region Invoke

        [Test]
        public async Task Invoke_CorrectData_NextRequestDelegateInvoked()
        {
            mockRequestDelegate.Setup(next => next.Invoke(mockHttpContext.Object));

            await errorHandlingMiddleware.Invoke(mockHttpContext.Object);

            mockRequestDelegate.Verify(next => next.Invoke(mockHttpContext.Object), Times.Once);
        }

        [Test]
        public async Task Invoke_ContextNotProvided_ExceptionShouldBeThrown()
        {
            Func<Task> result = async () => await errorHandlingMiddleware.Invoke(null);
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestCaseSource(nameof(validationExceptionCases))]
        public async Task Invoke_ValidationExceptionThrown_ExceptionResponseWithInvalidParameters(string validationErrorCode, string errorTitle, string errorType, int httpStatusCode)
        {
            var propertyName = "TestPropertyName";
            var validationException = new ValidationException(
                new List<ValidationFailure>
                {
                    new ValidationFailure(propertyName, ErrorMessage)
                    {
                        ErrorCode = validationErrorCode
                    }
                });

            mockHttpResponse.SetupSet(x => x.StatusCode = It.IsAny<int>()).Callback<int>(actualStatusCode => actualStatusCode.Should().Be(httpStatusCode));
            mockHttpResponse.SetupSet(x => x.ContentType = It.IsAny<string>()).Callback<string>(actualContentType => actualContentType.Should().Be(ContentType));
            mockRequestDelegate.Setup(next => next.Invoke(mockHttpContext.Object)).ThrowsAsync(validationException);

            await errorHandlingMiddleware.Invoke(mockHttpContext.Object);

            mockResponseHandler.Verify(x => x.HandleResponseAsync(It.IsAny<HttpContext>(), It.IsAny<Exception>(), It.IsAny<ExceptionResponse>()), Times.Once);
        }

        [Test]
        public async Task Invoke_UnhandledExceptionThrown_ExceptionResponse()
        {
            var unhandledException = new Exception(ErrorMessage);

            var expectedExceptionResponse = new ExceptionResponse
            {
                Title = ErrorTitles.InternalServerError,
                Type = ErrorTypes.InternalServerError,
                Detail = ErrorMessage,
                Instance = Path,
                TraceId = TraceId,
                Status = 500
            };

            mockHttpResponse.SetupSet(x => x.StatusCode = It.IsAny<int>()).Callback<int>(actualStatusCode => actualStatusCode.Should().Be(StatusCodes.Status500InternalServerError));
            mockHttpResponse.SetupSet(x => x.ContentType = It.IsAny<string>()).Callback<string>(actualContentType => actualContentType.Should().Be(ContentType));
            mockRequestDelegate.Setup(next => next.Invoke(mockHttpContext.Object)).ThrowsAsync(unhandledException);

            await errorHandlingMiddleware.Invoke(mockHttpContext.Object);

            mockResponseHandler.Verify(x => x.HandleResponseAsync(It.IsAny<HttpContext>(), It.IsAny<Exception>(), It.IsAny<ExceptionResponse>()), Times.Once);
        }

        [Test]
        public async Task Invoke_FileNotFoundExceptionThrown_ExceptionResponse()
        {
            var fileNotFoundException = new FileNotFoundException(ErrorMessage);

            var expectedExceptionResponse = new ExceptionResponse
            {
                Title = ErrorTitles.NotFoundError,
                Type = ErrorTypes.NotFound,
                Detail = ErrorMessage,
                Instance = Path,
                TraceId = TraceId,
                Status = 404
            };

            mockHttpResponse.SetupSet(x => x.StatusCode = It.IsAny<int>()).Callback<int>(actualStatusCode => actualStatusCode.Should().Be(StatusCodes.Status404NotFound));
            mockHttpResponse.SetupSet(x => x.ContentType = It.IsAny<string>()).Callback<string>(actualContentType => actualContentType.Should().Be(ContentType));
            mockRequestDelegate.Setup(next => next.Invoke(mockHttpContext.Object)).ThrowsAsync(fileNotFoundException);

            await errorHandlingMiddleware.Invoke(mockHttpContext.Object);

            mockResponseHandler.Verify(x => x.HandleResponseAsync(It.IsAny<HttpContext>(), It.IsAny<Exception>(), It.IsAny<ExceptionResponse>()), Times.Once);
        }

        [Test]
        public async Task Invoke_KeyNotFoundExceptionThrown_ExceptionResponse()
        {
            var keyNotFoundException = new KeyNotFoundException(ErrorMessage);

            var expectedExceptionResponse = new ExceptionResponse
            {
                Title = ErrorTitles.NotFoundError,
                Type = ErrorTypes.NotFound,
                Detail = ErrorMessage,
                Instance = Path,
                TraceId = TraceId,
                Status = 404
            };

            mockHttpResponse.SetupSet(x => x.StatusCode = It.IsAny<int>()).Callback<int>(actualStatusCode => actualStatusCode.Should().Be(StatusCodes.Status404NotFound));
            mockHttpResponse.SetupSet(x => x.ContentType = It.IsAny<string>()).Callback<string>(actualContentType => actualContentType.Should().Be(ContentType));
            mockRequestDelegate.Setup(next => next.Invoke(mockHttpContext.Object)).ThrowsAsync(keyNotFoundException);

            await errorHandlingMiddleware.Invoke(mockHttpContext.Object);

            mockResponseHandler.Verify(x => x.HandleResponseAsync(It.IsAny<HttpContext>(), It.IsAny<Exception>(), It.IsAny<ExceptionResponse>()), Times.Once);
        }

        [Test]
        public async Task Invoke_ArgumentExceptionThrown_ExceptionResponse()
        {
            var argumentException = new ArgumentException(ErrorMessage);

            var expectedExceptionResponse = new ExceptionResponse
            {
                Title = ErrorTitles.ArgumentError,
                Type = ErrorTypes.BadRequest,
                Detail = ErrorMessage,
                Instance = Path,
                TraceId = TraceId,
                Status = 400
            };

            mockHttpResponse.SetupSet(x => x.StatusCode = It.IsAny<int>()).Callback<int>(actualStatusCode => actualStatusCode.Should().Be(StatusCodes.Status404NotFound));
            mockHttpResponse.SetupSet(x => x.ContentType = It.IsAny<string>()).Callback<string>(actualContentType => actualContentType.Should().Be(ContentType));
            mockRequestDelegate.Setup(next => next.Invoke(mockHttpContext.Object)).ThrowsAsync(argumentException);

            await errorHandlingMiddleware.Invoke(mockHttpContext.Object);

            mockResponseHandler.Verify(x => x.HandleResponseAsync(It.IsAny<HttpContext>(), It.IsAny<Exception>(), It.IsAny<ExceptionResponse>()), Times.Once);
        }

        #endregion
    }
}