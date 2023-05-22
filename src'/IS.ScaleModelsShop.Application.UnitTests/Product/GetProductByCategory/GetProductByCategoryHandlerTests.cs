using AutoMapper;
using FluentAssertions;
using IS.ScaleModelsShop.Application.Exceptions;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductsByCategory;
using IS.ScaleModelsShop.Application.Profiles;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;
using System.Linq.Expressions;

namespace IS.ScaleModelsShop.Application.UnitTests.Product.GetProductByCategory
{
    public class GetProductByCategoryHandlerTests
    {
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private IMapper _mapper;
        private GetProductsByCategoryQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
            });

            _mapper = configurationProvider.CreateMapper();

            _handler = new GetProductsByCategoryQueryHandler(_mapper, _productRepositoryMock.Object,
                _categoryRepositoryMock.Object);
        }

        [Test]
        public void Constructor_WhenCalledWithNoProductRepository_ShouldThrowNewArgumentNullException()
        {
            Action result = () => new GetProductsByCategoryQueryHandler(_mapper, null, _categoryRepositoryMock.Object);

            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Constructor_WhenCalledWithNoCategoryRepository_ShouldThrowNewArgumentNullException()
        {
            Action result = () => new GetProductsByCategoryQueryHandler(_mapper, _productRepositoryMock.Object, null);

            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Constructor_WhenCalledWithNoMapper_ShouldThrowNewArgumentNullException()
        {
            Action result = () => new GetProductsByCategoryQueryHandler(null, _productRepositoryMock.Object, _categoryRepositoryMock.Object);

            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public async Task Handler_WhenCalledWithNonExistingManufacturer_ShouldThrowNotFoundException()
        {
            _categoryRepositoryMock
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Category, bool>>>()))
                .ReturnsAsync(false);

            Func<Task> result = async () =>
                await _handler.Handle(new GetProductsByCategoryQuery(), CancellationToken.None);

            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task Handler_WhenCalled_AnyShouldBeCalled()
        {
            _categoryRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Category, bool>>>()))
                .ReturnsAsync(true);

            await _handler.Handle(new GetProductsByCategoryQuery(), CancellationToken.None);

            _categoryRepositoryMock.Verify(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Category, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Handler_WhenCalled_FilterShouldBeCalled()
        {
            _categoryRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Category, bool>>>()))
                .ReturnsAsync(true);

            _productRepositoryMock.Setup(x => x.FilterAsync(It.IsAny<Expression<Func<Domain.Entities.Product, bool>>>(),
                It.IsAny<Expression<Func<Domain.Entities.Product, Domain.Entities.Product>>>(), CancellationToken.None,
                true))
                .ReturnsAsync(new List<Domain.Entities.Product> { new Domain.Entities.Product() }.AsEnumerable());

            await _handler.Handle(new GetProductsByCategoryQuery(), CancellationToken.None);

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