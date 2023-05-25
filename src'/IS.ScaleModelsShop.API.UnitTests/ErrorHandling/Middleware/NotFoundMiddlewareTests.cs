using FluentAssertions;
using IS.ScaleModelsShop.API.Common;
using IS.ScaleModelsShop.API.Middleware;
using IS.ScaleModelsShop.API.Responses;
using IS.ScaleModelsShop.API.UnitTests.TestEntities.Mock.Http;
using IS.ScaleModelsShop.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace IS.ScaleModelsShop.API.UnitTests.ErrorHandling.Middleware;

[TestFixture]
public class NotFoundMiddlewareTests
{
    private const string Path = "/TestPath";
    private const string TraceId = "TestTraceId";

    private Mock<HttpContext> _mockHttpContext;
    private Mock<HttpResponse> _mockHttpResponse;
    private Mock<RequestDelegate> _mockRequestDelegate;
    private Mock<IResponseHandler> _mockResponseHandler;
    private HttpRequest _fakeHttpRequest;
    private NotFoundMiddleware _notFoundMiddleware;

    [SetUp]
    public void Setup()
    {
        _mockHttpContext = new Mock<HttpContext>();
        _mockRequestDelegate = new Mock<RequestDelegate>();
        _mockResponseHandler = new Mock<IResponseHandler>();

        _fakeHttpRequest = new FakeHttpRequest
        {
            Path = new PathString(Path)
        };

        _mockHttpResponse = new Mock<HttpResponse>();

        _mockHttpContext = new Mock<HttpContext>();
        _mockHttpContext.SetupGet(x => x.TraceIdentifier).Returns(TraceId);
        _mockHttpContext.SetupGet(x => x.Request).Returns(_fakeHttpRequest);
        _mockHttpContext.SetupGet(x => x.Response).Returns(_mockHttpResponse.Object);

        _notFoundMiddleware = new NotFoundMiddleware(_mockRequestDelegate.Object, _mockResponseHandler.Object);
    }

    #region Constructor

    [Test]
    public void Constructor_WhenCalledWithValidArguments_InstanceShouldBeCreated()
    {
        _notFoundMiddleware.Should().NotBeNull();
    }

    [Test]
    public void Constructor_WhenInstanceCreatedWithNullRequestDelegate_ExceptionShouldBeThrown()
    {
        Func<NotFoundMiddleware> result = () => new NotFoundMiddleware(null, _mockResponseHandler.Object);
        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenInstanceCreatedWithNullResponseHandler_ExceptionShouldBeThrown()
    {
        Func<NotFoundMiddleware> result = () => new NotFoundMiddleware(_mockRequestDelegate.Object, null);
        result.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region Invoke

    [Test]
    public async Task Invoke_WhenContextNull_NextRequestDelegateInvoked()
    {
        Func<Task> result = async () => await _notFoundMiddleware.Invoke(null);
        await result.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task Invoke_CorrectData_NextRequestDelegateInvoked()
    {
        _mockRequestDelegate.Setup(next => next.Invoke(_mockHttpContext.Object));

        await _notFoundMiddleware.Invoke(_mockHttpContext.Object);

        _mockRequestDelegate.Verify(next => next.Invoke(_mockHttpContext.Object), Times.Once);
    }

    #endregion

    [Test]
    public async Task Invoke_ValidationExceptionThrown_ResponseHandlerShouldBeCalled()
    {
        var errorMessageName = "TestErrorMessageName";
        var errorMessageKey = "TestErrorMessageName";

        var notFoundExceptions = new NotFoundException(errorMessageName, errorMessageKey);

        _mockHttpResponse.SetupSet(x => x.StatusCode = It.IsAny<int>()).Callback<int>(actualStatusCode => actualStatusCode.Should().Be(StatusCodes.Status404NotFound));
        _mockRequestDelegate.Setup(next => next.Invoke(_mockHttpContext.Object)).ThrowsAsync(notFoundExceptions);

        await _notFoundMiddleware.Invoke(_mockHttpContext.Object);

        _mockResponseHandler.Verify(x => x.HandleResponseAsync(It.IsAny<HttpContext>(), It.IsAny<NotFoundException>(), It.IsAny<ExceptionResponse>()), Times.Once);
    }
}