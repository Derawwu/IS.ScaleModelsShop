using AutoMapper;
using FluentAssertions;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Queries.GetAllManufacturersList;
using IS.ScaleModelsShop.Application.Models.Queries;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;

namespace IS.ScaleModelsShop.Application.UnitTests.Manufacturer.GetManufacturers;

public class GetAllManufacturersQueryHandlerTest
{
    private GetAllManufacturersQueryHandler _handler;
    private Mock<IManufacturerRepository> _manufacturerRepositoryMock;
    private Mock<IMapper> _mapperMock;

    [SetUp]
    public void Setup()
    {
        _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();

        _mapperMock = new Mock<IMapper>();

        _handler = new GetAllManufacturersQueryHandler(_mapperMock.Object, _manufacturerRepositoryMock.Object);
    }

    [Test]
    public void Constructor_WhenCalledWithNoRepository_ShouldThrowNewArgumentNullException()
    {
        Action result = () => new GetAllManufacturersQueryHandler(_mapperMock.Object, null);

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
            new List<Domain.Entities.Manufacturer> { new() }.AsEnumerable());

        await _handler.Handle(new GetAllManufacturersQuery(), CancellationToken.None);

        _manufacturerRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task Handle_WhenCalledWithNullRequest_ShouldThrowNewArgumentNullException()
    {
        Func<Task> result = async () => await _handler.Handle(null, CancellationToken.None);

        await result.Should().ThrowAsync<ArgumentNullException>();
    }
}