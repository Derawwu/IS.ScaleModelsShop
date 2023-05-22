using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using IS.ScaleModelsShop.Application.Exceptions;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductByManufacturer;
using IS.ScaleModelsShop.Application.Profiles;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;

namespace IS.ScaleModelsShop.Application.UnitTests.Product.GetProductByManufacturer
{
    public class GetProductByManufacturerTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IManufacturerRepository> _manufacturerRepositoryMock;
        private IMapper _mapper;
        private GetProductsByManufacturerQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
            });

            _mapper = configurationProvider.CreateMapper();

            _handler = new GetProductsByManufacturerQueryHandler(_mapper, _productRepositoryMock.Object,
                _manufacturerRepositoryMock.Object);
        }

        [Test]
        public void Constructor_WhenCalledWithNoProductRepository_ShouldThrowNewArgumentNullException()
        {
            Action result = () => new GetProductsByManufacturerQueryHandler(_mapper, null, _manufacturerRepositoryMock.Object);

            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Constructor_WhenCalledWithNoManufacturerRepository_ShouldThrowNewArgumentNullException()
        {
            Action result = () => new GetProductsByManufacturerQueryHandler(_mapper, _productRepositoryMock.Object, null);

            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Constructor_WhenCalledWithNoMapper_ShouldThrowNewArgumentNullException()
        {
            Action result = () => new GetProductsByManufacturerQueryHandler(null, _productRepositoryMock.Object, _manufacturerRepositoryMock.Object);

            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public async Task Handler_WhenCalledWithNonExistingManufacturer_ShouldThrowNotFoundException()
        {
            _manufacturerRepositoryMock
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Manufacturer, bool>>>()))
                .ReturnsAsync(false);

            Func<Task> result = async () =>
                await _handler.Handle(new GetProductsByManufacturerQuery(), CancellationToken.None);

            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task Handler_WhenCalled_AnyShouldBeCalled()
        {
            _manufacturerRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Manufacturer, bool>>>()))
                .ReturnsAsync(true);

            await _handler.Handle(new GetProductsByManufacturerQuery(), CancellationToken.None);

            _manufacturerRepositoryMock.Verify(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Manufacturer, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Handler_WhenCalled_FilterShouldBeCalled()
        {
            _manufacturerRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Manufacturer, bool>>>()))
                .ReturnsAsync(true);

            _productRepositoryMock.Setup(x => x.FilterAsync(It.IsAny<Expression<Func<Domain.Entities.Product, bool>>>(),
                It.IsAny<Expression<Func<Domain.Entities.Product, Domain.Entities.Product>>>(), CancellationToken.None,
                true))
                .ReturnsAsync( new List<Domain.Entities.Product> { new Domain.Entities.Product() }.AsEnumerable());

            await _handler.Handle(new GetProductsByManufacturerQuery(), CancellationToken.None);

            _productRepositoryMock.Verify(x => x.FilterAsync(It.IsAny<Expression<Func<Domain.Entities.Product, bool>>>(),
                It.IsAny<Expression<Func<Domain.Entities.Product, Domain.Entities.Product>>>(), CancellationToken.None,
                true), Times.Once);
        }

        [Test]
        public async Task Handler_WhenCalledWithNullRequest_ShouldThrowNewArgumentNullException()
        {
            Func<Task> result = async () => await _handler.Handle(null, CancellationToken.None);

            await result.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}