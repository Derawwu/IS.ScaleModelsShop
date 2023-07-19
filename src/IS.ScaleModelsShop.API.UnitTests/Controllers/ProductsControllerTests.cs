using AutoMapper;
using FluentAssertions;
using IS.ScaleModelsShop.API.Contracts.Product;
using IS.ScaleModelsShop.API.Contracts.Product.UpdateProduct;
using IS.ScaleModelsShop.API.Controllers;
using IS.ScaleModelsShop.API.UnitTests.TestEntities;
using IS.ScaleModelsShop.Application.Features.Products.Commands.CreateProduct;
using IS.ScaleModelsShop.Application.Features.Products.Commands.DeleteProduct;
using IS.ScaleModelsShop.Application.Features.Products.Commands.UpdateProduct;
using IS.ScaleModelsShop.Application.Models.Queries;
using IS.ScaleModelsShop.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData.ModelBuilder;
using Microsoft.OData.UriParser;
using Moq;

namespace IS.ScaleModelsShop.API.UnitTests.Controllers;

public class ProductsControllerTests
{
    private Mock<IMediator> _mockMediator;
    private Mock<IMapper> _mockMapper;
    private ProductsController _controller;
    private Guid _fakeProductId;
    private UpdateProductCommand _fakeProduct;

    [SetUp]
    public void Setup()
    {
        _fakeProductId = new Guid("00000000-0000-0000-0000-000000000001");

        _mockMapper = new Mock<IMapper>();

        _fakeProduct = new UpdateProductCommand()
        {
            Id = _fakeProductId,
            Name = nameof(UpdateProductCommand.Name),
            Description = nameof(UpdateProductCommand.Description),
            Price = 1,
            CategoryId = Guid.NewGuid(),
            ManufacturerId = Guid.NewGuid()
        };

        _mockMediator = new Mock<IMediator>();

        _controller = new ProductsController(_mockMediator.Object, _mockMapper.Object);
    }

    #region Constructor

    [Test]
    public void Constructor_WhenCalledWithNullMapper_ShouldThrowArgumentNullException()
    {
        var result = () => new ProductsController(_mockMediator.Object, null);

        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenCalledWithNullMediator_ShouldThrowArgumentNullException()
    {
        var result = () => new ProductsController(null, _mockMapper.Object);

        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenCalledWithValidArguments_ShouldBeInitialized()
    {
        _controller.Should().NotBeNull();
    }

    #endregion

    #region GetProducts

    [Test]
    public async Task GetProducts_WhenCalled_ShouldReturnOkObjectResult()
    {
        _mockMediator.Setup(x =>
            x.Send(It.IsAny<GetProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PaginatedCollection<ProductModel>(new List<ProductModel> { new ProductModel() }, 1));

        var modelBuilder = new ODataConventionModelBuilder();
        var entitySetToSetup = modelBuilder.EntitySet<Domain.Entities.Product>("Product");
        entitySetToSetup.EntityType.HasKey(entity => entity.Id);
        var edmModel = modelBuilder.GetEdmModel();

        const string routeName = "odata";
        var entitySet = edmModel.EntityContainer.FindEntitySet("Product");
        var path = new ODataPath(new EntitySetSegment(entitySet));

        var request = RequestFactory.Create(
            "GET",
            "http://localhost/api?$top=1",
            dataOptions => dataOptions.AddRouteComponents(routeName, edmModel));

        request.ODataFeature().Model = edmModel;
        request.ODataFeature().Path = path;
        request.ODataFeature().RoutePrefix = routeName;

        var odataQueryContext = new ODataQueryContext(edmModel, typeof(Domain.Entities.Product), new ODataPath());
        var odataQueryOptions = new ODataQueryOptions<Domain.Entities.Product>(odataQueryContext, request);

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.SetupGet(x => x.Response.Headers).Returns(new Mock<IHeaderDictionary>().Object);
        _controller.ControllerContext.HttpContext = mockHttpContext.Object;
        var result = await _controller.GetProducts(odataQueryOptions);

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var model = ((OkObjectResult)result).Value;
        model.Should().NotBeNull();
    }

    [Test]
    public async Task GetProducts_WhenCalledWithoutEntitiesInStorage_ShouldReturnOkObjectResultWithEmptyList()
    {
        var expectedTotalCount = 0;

        _mockMediator.Setup(x =>
            x.Send(It.IsAny<GetProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PaginatedCollection<ProductModel>(new List<ProductModel> { }, expectedTotalCount));

        var modelBuilder = new ODataConventionModelBuilder();
        var entitySetToSetup = modelBuilder.EntitySet<Domain.Entities.Product>("Product");
        entitySetToSetup.EntityType.HasKey(entity => entity.Id);
        var edmModel = modelBuilder.GetEdmModel();

        const string routeName = "odata";
        var entitySet = edmModel.EntityContainer.FindEntitySet("Product");
        var path = new ODataPath(new EntitySetSegment(entitySet));

        var request = RequestFactory.Create(
            "GET",
            "http://localhost/api?$top=1",
            dataOptions => dataOptions.AddRouteComponents(routeName, edmModel));

        request.ODataFeature().Model = edmModel;
        request.ODataFeature().Path = path;
        request.ODataFeature().RoutePrefix = routeName;

        var odataQueryContext = new ODataQueryContext(edmModel, typeof(Domain.Entities.Product), new ODataPath());
        var odataQueryOptions = new ODataQueryOptions<Domain.Entities.Product>(odataQueryContext, request);

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.SetupGet(x => x.Response.Headers).Returns(new Mock<IHeaderDictionary>().Object);
        _controller.ControllerContext.HttpContext = mockHttpContext.Object;

        var result = await _controller.GetProducts(odataQueryOptions);

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var model = (IEnumerable<ProductModel>)((OkObjectResult)result).Value;
        model.Count().Should().Be(expectedTotalCount);
        model.Should().BeOfType<List<ProductModel>>();
        model.Should().BeEquivalentTo(Array.Empty<ProductModel>());
    }

    #endregion

    #region CreateProduct

    [Test]
    public async Task CreateProduct_WhenCalled_ShouldReturnOkObjectResult()
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProductModel());

        var result = await _controller.CreateProduct(new CreateProductCommand());

        result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

        var resultProductModel = (ProductModel)((OkObjectResult)result).Value;
        resultProductModel.Should().NotBeNull().And.BeOfType<ProductModel>();
    }

    #endregion

    #region UpdateProduct

    [Test]
    public async Task UpdateManufacturer_WhenCalled_ShouldReturnNoContentResult()
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockMapper.Setup(x => x.Map<UpdateProductCommand>(It.IsAny<UpdateProductModel>()))
            .Returns(_fakeProduct);

        var result = await _controller.UpdateProduct(_fakeProductId, new UpdateProductModel());

        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
    }

    #endregion

    #region DeleteProduct

    [Test]
    public async Task DeleteManufacturer_WhenCalled_ShouldReturnNoContentResult()
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _controller.DeleteProduct(_fakeProductId);

        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
    }

    #endregion

    //private IEnumerable<ProductModel> GetExpectedProducts()
    //{
    //    var product
    //}
}