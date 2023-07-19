using FluentAssertions;
using IS.ScaleModelsShop.API.Contracts.Category;
using IS.ScaleModelsShop.API.Controllers;
using IS.ScaleModelsShop.Application.Features.Categories.Commands.CreateCategory;
using IS.ScaleModelsShop.Application.Models.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace IS.ScaleModelsShop.API.UnitTests.Controllers;

public class CategoryControllerTests
{
    private Mock<IMediator> _mockMediator;
    private CategoriesController _controller;
    private CreateCategoryCommand _fakeCreateCategoryCommand;

    [SetUp]
    public void Setup()
    {
        _mockMediator = new Mock<IMediator>();
        _mockMediator.Setup(x => x.Send(It.IsAny<CreateCategoryCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategoryModel());

        _fakeCreateCategoryCommand = new CreateCategoryCommand
        {
            Name = "A category"
        };

        _controller = new CategoriesController(_mockMediator.Object);
    }

    #region Constructor

    [Test]
    public void Constructor_WhenCalledWithoutMediator_ShouldThrowNullArgumentException()
    {
        var result = () => new CategoriesController(null);

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
        _mockMediator.Setup(x => x.Send(It.IsAny<GetAllCategoriesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CategoryModel> { new() });

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.SetupGet(x => x.Response.Headers).Returns(new Mock<IHeaderDictionary>().Object);
        _controller.ControllerContext.HttpContext = mockHttpContext.Object;
        var result = await _controller.GetAll();

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var model = ((OkObjectResult)result).Value;
        model.Should().NotBeNull();
    }

    [Test]
    public async Task GetAllCategoriesList_WhenCalledWithoutCategoriesInStorage_ShouldReturnOkObjectResultWithoutEntities()
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<GetAllCategoriesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CategoryModel> { });

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.SetupGet(x => x.Response.Headers).Returns(new Mock<IHeaderDictionary>().Object);
        _controller.ControllerContext.HttpContext = mockHttpContext.Object;
        var result = await _controller.GetAll();

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var model = (List<CategoryModel>)((OkObjectResult)result).Value;
        model.Should().BeEquivalentTo(Array.Empty<CategoryModel>());
    }

    #endregion

    #region CreateCategory

    [Test]
    public async Task CreateCategory_WhenCalledWIthValidRequest_ShouldReturnOkObjectResult()
    {
        var result = await _controller.CreateCategory(new CreateCategoryCommand());

        result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

        var resultOrganizationModel = (CategoryModel)((OkObjectResult)result).Value;
        resultOrganizationModel.Should().NotBeNull().And.BeOfType<CategoryModel>();
    }

    #endregion
}