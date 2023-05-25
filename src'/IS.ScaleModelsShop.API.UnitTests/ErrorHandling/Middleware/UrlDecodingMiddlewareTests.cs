using FluentAssertions;
using IS.ScaleModelsShop.API.Middleware;
using Microsoft.AspNetCore.Http;
using Moq;

namespace IS.ScaleModelsShop.API.UnitTests.ErrorHandling.Middleware;

public class UrlDecodingMiddlewareTests
{
    private const string Path = "/test%20path";

    private Mock<HttpContext> _mockHttpContext;
    private Mock<HttpRequest> _mockHttpRequest;
    private Mock<RequestDelegate> _mockRequestDelegate;
    private UrlDecodingMiddleware _middleware;

    [SetUp]
    public void Setup()
    {
        _mockHttpContext = new Mock<HttpContext>();
        _mockHttpRequest = new Mock<HttpRequest>();
        _mockRequestDelegate = new Mock<RequestDelegate>();

        _mockHttpContext = new Mock<HttpContext>();

        _mockHttpRequest.SetupGet(r => r.Path).Returns(new PathString(Path));
        _mockHttpContext.SetupGet(c => c.Request).Returns(_mockHttpRequest.Object);

        _middleware = new UrlDecodingMiddleware(_mockRequestDelegate.Object);
    }

    #region Constructor

    [Test]
    public void Constructor_WhenCalledWithValidArguments_InstanceShouldBeCreated()
    {
        _middleware.Should().NotBeNull();
    }

    [Test]
    public void Constructor_WhenInstanceCreatedWithNullRequestDelegate_ExceptionShouldBeThrown()
    {
        Func<UrlDecodingMiddleware> result = () => new UrlDecodingMiddleware(null);
        result.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region Invoke

    [Test]
    public async Task Invoke_WhenContextNull_NextRequestDelegateInvoked()
    {
        Func<Task> result = async () => await _middleware.InvokeAsync(null);
        await result.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task Invoke_WhenCalled_ShouldDecodePath()
    {
        var decodedPathValue = "/test path";

        await _middleware.InvokeAsync(_mockHttpContext.Object);

        _mockHttpRequest.VerifySet(r => r.Path = new PathString(decodedPathValue), Times.Once);
    }

    #endregion
}
