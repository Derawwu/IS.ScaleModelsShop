using AutoMapper;
using FluentAssertions;
using IS.ScaleModelsShop.Application.Features.Categories.Queries.GetAllCategoriesList;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Queries.GetAllManufacturersList;
using IS.ScaleModelsShop.Application.Profiles;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;

namespace IS.ScaleModelsShop.Application.UnitTests.Manufacturer.GetManufacturers
{
    public class GetAllManufacturersQueryHandlerTest
    {
        private GetAllManufacturersQueryHandler _handler;
        private Mock<IManufacturerRepository> _manufacturerRepositoryMock;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
            });

            _mapper = configurationProvider.CreateMapper();

            _handler = new GetAllManufacturersQueryHandler(_mapper, _manufacturerRepositoryMock.Object);
        }

        [Test]
        public void Constructor_WhenCalledWithNoRepository_ShouldThrowNewArgumentNullException()
        {
            Action result = () => new GetAllManufacturersQueryHandler(_mapper, null);

            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Constructor_WhenCalledWithNoMapper_ShouldThrowNewArgumentNullException()
        {
            Action result = () => new GetAllManufacturersQueryHandler(null, _manufacturerRepositoryMock.Object);

            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public async Task Handle_WhenCalled_GetAllShouldBeCalled()
        {
            _manufacturerRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(
                new List<Domain.Entities.Manufacturer>{ new Domain.Entities.Manufacturer()}.AsEnumerable());

            await _handler.Handle(new GetAllManufacturersListQuery(), CancellationToken.None);

            _manufacturerRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task Handle_WhenCalledWithNullRequest_ShouldThrowNewArgumentNullException()
        {
            Func<Task> result = async () => await _handler.Handle(null, CancellationToken.None);

            await result.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}