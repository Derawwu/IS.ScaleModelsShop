using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using IS.ScaleModelsShop.Application.Features.Products.Commands.CreateProduct;
using IS.ScaleModelsShop.Application.Profiles;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;

namespace IS.ScaleModelsShop.Application.UnitTests.Product.CreateProduct
{
    public class CreateProductCommandHandlerTest
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private Mock<IManufacturerRepository> _manufacturerRepositoryMock;
        private IMapper _mapper;
        private CreateProductCommandHandler _handler;
        private CreateProductCommand _fakeProduct;

        [SetUp]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();

            _categoryRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Category, bool>>>()))
                .ReturnsAsync(true);
            _manufacturerRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Manufacturer, bool>>>()))
                .ReturnsAsync(true);

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
            });

            _mapper = configurationProvider.CreateMapper();

            _handler = new CreateProductCommandHandler(_mapper, _productRepositoryMock.Object,
                _categoryRepositoryMock.Object);

            _fakeProduct = new CreateProductCommand()
            {
                CategoryId = Guid.Empty,
                Name = nameof(CreateProductCommand.Name),
                ManufacturerId = Guid.Empty,
                Description = nameof(CreateProductCommand.Description),
                Price = Decimal.One
            };
        }

        [Test]
        public void Constructor_WhenCalledWithNoProductRepository_ShouldThrowNewArgumentNullException()
        {
            Action result = () => new CreateProductCommandHandler(_mapper, null, _categoryRepositoryMock.Object);
            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Constructor_WhenCalledWithNoCategoryRepository_ShouldThrowNewArgumentNullException()
        {
            Action result = () => new CreateProductCommandHandler(_mapper, _productRepositoryMock.Object, null);
            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Constructor_WhenCalledWithNoMapper_ShouldThrowNewArgumentNullException()
        {
            Action result = () => new CreateProductCommandHandler(null, _productRepositoryMock.Object, _categoryRepositoryMock.Object);
            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public async Task Handler_WhenCalledCreateProduct_ShouldCreateProduct()
        {
            await _handler.Handle(_fakeProduct, CancellationToken.None);

            _productRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Product>()), Times.Once);
        }

        [Test]
        public async Task Handler_WhenCalledWIthNullRequest_ShouldThrowNewArgumentNullException()
        {
            Func<Task> result = async () => await _handler.Handle(null, CancellationToken.None);

            await result.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}