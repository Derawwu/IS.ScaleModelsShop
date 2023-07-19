using FluentAssertions;
using IS.ScaleModelsShop.Application.Exceptions;
using IS.ScaleModelsShop.Application.Features.Products.Commands.DeleteProduct;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;

namespace IS.ScaleModelsShop.Application.UnitTests.Product.DeleteProduct
{
    public class DeleteProductCommandHandlerTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private DeleteProductCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IProductRepository>();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(new Domain.Entities.Product());

            _handler = new DeleteProductCommandHandler(_productRepositoryMock.Object);
        }

        [Test]
        public void Constructor_WhenCalledWithNoProductRepository_ShouldThrowNewArgumentNullException()
        {
            Action result = () => new DeleteProductCommandHandler(null);
            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public async Task Handler_WhenCalledDeleteProduct_ShouldCreateProduct()
        {
            await _handler.Handle(new DeleteProductCommand(), CancellationToken.None);

            _productRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public async Task Handler_WhenCalledWIthNullRequest_ShouldThrowNewArgumentNullException()
        {
            Func<Task> result = async () => await _handler.Handle(null, CancellationToken.None);

            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Test]
        public async Task Handle_WhenCalledWithNullProduct_ShouldThrowNotFoundException()
        {
            _productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync((Domain.Entities.Product)null);

            Func<Task> result = async () => await _handler.Handle(new DeleteProductCommand(), CancellationToken.None);

            await result.Should().ThrowAsync<EntityNotFoundException>();
        }
    }
}
