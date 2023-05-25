using FluentAssertions;
using IS.ScaleModelsShop.API.Controllers;
using IS.ScaleModelsShop.Application.Features.Categories.Commands.CreateCategory;
using IS.ScaleModelsShop.Application.Features.Categories.Queries.GetAllCategoriesList;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace IS.ScaleModelsShop.API.UnitTests.Controllers;

public class CategoryControllerTests
{
    private Mock<IMediator> _mockMediator;
    private CategoryController _controller;
    private CreateCategoryCommand _fakeCreateCategoryCommand;

    [SetUp]
    public void Setup()
    {
        _mockMediator = new Mock<IMediator>();
        _mockMediator.Setup(x => x.Send(It.IsAny<CreateCategoryCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreateCategoryDTO());

        _fakeCreateCategoryCommand = new CreateCategoryCommand
        {
            Name = "A category"
        };

        _controller = new CategoryController(_mockMediator.Object);
    }

    #region Constructor

    [Test]
    public void Constructor_WhenCalledWithoutMediator_ShouldThrowNullArgumentException()
    {
        var result = () => new CategoryController(null);

        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenCalledWithMediator_ShouldBeInitialized()
    {
        _controller.Should().NotBeNull();
    }

    #endregion

    #region GetAllCategories

    [Test]
    public async Task GetAllCategoriesList_WhenCalled_ShouldReturnOkObjectResult()
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<GetAllCategoriesListQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CategoryListViewModel> { new() });

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.SetupGet(x => x.Response.Headers).Returns(new Mock<IHeaderDictionary>().Object);
        _controller.ControllerContext.HttpContext = mockHttpContext.Object;
        var result = await _controller.GetAllCategoriesList();

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var model = ((OkObjectResult)result).Value;
        model.Should().NotBeNull();
    }

    [Test]
    public async Task GetAllCategoriesList_WhenCalledWithoutCategoriesInStorage_ShouldReturnNoContentResult()
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<GetAllCategoriesListQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CategoryListViewModel> { });

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.SetupGet(x => x.Response.Headers).Returns(new Mock<IHeaderDictionary>().Object);
        _controller.ControllerContext.HttpContext = mockHttpContext.Object;
        var result = await _controller.GetAllCategoriesList();

        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
    }

    #endregion

    #region CreateCategory

    [Test]
    public async Task CreateCategory_WhenCalledWIthValidRequest_ShouldReturnOkObjectResult()
    {
        var result = await _controller.CreateCategory(new CreateCategoryCommand());

        result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

        var resultOrganizationModel = (CreateCategoryDTO)((OkObjectResult)result).Value;
        resultOrganizationModel.Should().NotBeNull().And.BeOfType<CreateCategoryDTO>();
    }

    #endregion
}