using AutoMapper;
using FluentAssertions;
using IS.ScaleModelsShop.API.Controllers;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.UpdateManufacturer;
using IS.ScaleModelsShop.Application.Features.Products.Commands.CreateProduct;
using IS.ScaleModelsShop.Application.Features.Products.Commands.DeleteProduct;
using IS.ScaleModelsShop.Application.Features.Products.Commands.UpdateProduct;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetAllProductsPaginated;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductByManufacturer;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductByName;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductsByCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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

        _mockMapper.Setup(x => x.Map<UpdateProductCommand>(It.IsAny<UpdateProductDTO>()))
            .Returns(_fakeProduct);

        _mockMediator = new Mock<IMediator>();

        _mockMediator.Setup(x => x.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetProductDTO());
        _mockMediator.Setup(x => x.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockMediator.Setup(x => x.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

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

    #region GetAllProductsPaginated

    [Test]
    public async Task GetAllProductsPaginated_WhenCalled_ShouldReturnOkObjectResult()
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<GetAllProductsPaginatedQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetPaginatedProductViewModel
            {
                Page = 1,
                Size = 1,
                Products = new List<GetProductDTO>
                {
                    new()
                }
            });

        var result = await _controller.GetProductsPaginated(1, 1);

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var model = (GetPaginatedProductViewModel)((OkObjectResult)result).Value;
        model.Should().NotBeNull();
        model.Products.Should().BeOfType<List<GetProductDTO>>();
    }

    [Test]
    public async Task GetAllProductsPaginated_WhenCalledWithoutEntitiesInStorage_ShouldReturnNoContentResult()
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<GetAllProductsPaginatedQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetPaginatedProductViewModel
            {
                Page = 1,
                Size = 1,
                Products = new List<GetProductDTO>()
            });

        var result = await _controller.GetProductsPaginated(1, 1);

        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
    }

    #endregion

    #region GetProductByCategoryName

    [Test]
    public async Task GetProductsByCategoryName_WhenCalled_ShouldReturnOkObjectResult()
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<GetProductsByCategoryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProductsDTO> { new ProductsDTO() });

        var result = await _controller.GetProductsByCategory(Guid.Empty);

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var model = (List<ProductsDTO>)((OkObjectResult)result).Value;
        model.Should().NotBeNull();
        model.Should().BeOfType<List<ProductsDTO>>();
    }

    [Test]
    public async Task GetProductsByCategoryName_WhenCalledWithoutEntitiesInStorage_ShouldReturnNoContentResult()
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<GetProductsByCategoryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProductsDTO> { });

        var result = await _controller.GetProductsByCategory(Guid.Empty);

        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
    }

    #endregion

    #region GetProductByManufacturerName

    [Test]
    public async Task GetProductsByManufacturerName_WhenCalled_ShouldReturnOkObjectResult()
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<GetProductsByManufacturerQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProductsDTO> { new ProductsDTO() });

        var result = await _controller.GetProductsByManufacturer(Guid.Empty);

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var model = (List<ProductsDTO>)((OkObjectResult)result).Value;
        model.Should().NotBeNull();
        model.Should().BeOfType<List<ProductsDTO>>();
    }

    [Test]
    public async Task GetProductsByManufacturerName_WhenCalledWithoutEntitiesInStorage_ShouldReturnNoContentResult()
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<GetProductsByManufacturerQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProductsDTO> { });

        var result = await _controller.GetProductsByManufacturer(Guid.Empty);

        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
    }

    #endregion

    #region GetProductByProductName

    [Test]
    public async Task GetProductByName_WhenCalled_ShouldReturnOkObjectResult()
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<GetProductByNameQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetProductByNameViewModel());

        var result = await _controller.GetProductByName("A product name");

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var model = ((OkObjectResult)result).Value;
        model.Should().NotBeNull();
        model.Should().BeOfType<GetProductByNameViewModel>();
    }

    #endregion

    #region CreateProduct

    [Test]
    public async Task CreateProduct_WhenCalled_ShouldReturnOkObjectResult()
    {
        var result = await _controller.CreateProduct(new CreateProductCommand());

        result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

        var resultProductModel = (GetProductDTO)((OkObjectResult)result).Value;
        resultProductModel.Should().NotBeNull().And.BeOfType<GetProductDTO>();
    }

    #endregion

    #region UpdateProduct

    [Test]
    public async Task UpdateManufacturer_WhenCalled_ShouldReturnNoContentResult()
    {
        var result = await _controller.UpdateProduct(_fakeProductId, new UpdateProductDTO());

        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
    }

    #endregion

    #region DeleteProduct

    [Test]
    public async Task DeleteManufacturer_WhenCalled_ShouldReturnNoContentResult()
    {
        var result = await _controller.DeleteProduct(_fakeProductId);

        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
    }

    #endregion
}