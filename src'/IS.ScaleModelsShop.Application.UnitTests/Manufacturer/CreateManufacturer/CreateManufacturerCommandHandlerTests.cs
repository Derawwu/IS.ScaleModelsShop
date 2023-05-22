using AutoMapper;
using FluentAssertions;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer;
using IS.ScaleModelsShop.Application.Profiles;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;

namespace IS.ScaleModelsShop.Application.UnitTests.Manufacturer.CreateManufacturer;

public class CreateManufacturerCommandHandlerTests
{
    private CreateManufacturerCommand _fakeManufacturer;
    private CreateManufacturerCommandHandler _handler;
    private Mock<IManufacturerRepository> _manufacturerRepositoryMock;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();

        var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MapperProfile>(); });

        _mapper = configurationProvider.CreateMapper();

        _fakeManufacturer = new CreateManufacturerCommand
        {
            Name = nameof(CreateManufacturerCommand)
        };

        _handler = new CreateManufacturerCommandHandler(_mapper, _manufacturerRepositoryMock.Object);
    }

    [Test]
    public void Constructor_WhenCalledWithNoRepository_ShouldThrowNewArgumentNullException()
    {
        Action result = () => new CreateManufacturerCommandHandler(_mapper, null);

        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenCalledWithNoMapper_ShouldThrowNewArgumentNullException()
    {
        Action result = () => new CreateManufacturerCommandHandler(null, _manufacturerRepositoryMock.Object);

        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public async Task Handler_WhenCalledCreateCategory_ShouldCreateCategory()
    {
        await _handler.Handle(_fakeManufacturer, CancellationToken.None);

        _manufacturerRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Manufacturer>()), Times.Once);
    }

    [Test]
    public async Task Handler_WhenCalledWIthNullRequest_ShouldThrowNewArgumentNullException()
    {
        Func<Task> result = async () => await _handler.Handle(null, CancellationToken.None);

        await result.Should().ThrowAsync<ArgumentNullException>();
    }
}