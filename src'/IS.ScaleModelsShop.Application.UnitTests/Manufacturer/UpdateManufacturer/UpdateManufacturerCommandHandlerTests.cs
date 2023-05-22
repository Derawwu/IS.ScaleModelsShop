using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using IS.ScaleModelsShop.Application.Exceptions;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.UpdateManufacturer;
using IS.ScaleModelsShop.Application.Profiles;
using IS.ScaleModelsShop.Application.Repositories;
using Moq;

namespace IS.ScaleModelsShop.Application.UnitTests.Manufacturer.UpdateManufacturer;

public class UpdateManufacturerCommandHandlerTests
{
    private Domain.Entities.Manufacturer _fakeManufacturer;
    private UpdateManufacturerCommandHandler _handler;
    private Mock<IManufacturerRepository> _manufacturerRepositoryMock;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _fakeManufacturer = new Domain.Entities.Manufacturer
        {
            Name = nameof(Domain.Entities.Manufacturer),
            Id = Guid.Empty
        };

        _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();
        _manufacturerRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_fakeManufacturer);

        var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MapperProfile>(); });

        _mapper = configurationProvider.CreateMapper();

        _handler = new UpdateManufacturerCommandHandler(_manufacturerRepositoryMock.Object, _mapper);
    }

    [Test]
    public void Constructor_WhenCalledWithNoManufacturerRepository_ShouldThrowArgumentNullException()
    {
        Action result = () => new UpdateManufacturerCommandHandler(null, _mapper);

        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenCalledWithNoMapper_ShouldThrowArgumentNullException()
    {
        Action result = () => new UpdateManufacturerCommandHandler(_manufacturerRepositoryMock.Object, null);

        result.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public async Task Handle_WhenCalled_UpdateAsyncShouldBeCalled()
    {
        _manufacturerRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Entities.Manufacturer, bool>>>()))
            .ReturnsAsync(false);

        await _handler.Handle(new UpdateManufacturerCommand(), CancellationToken.None);

        _manufacturerRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Domain.Entities.Manufacturer>()),
            Times.Once);
    }

    [Test]
    public async Task Handle_WhenCalledWithNullRequest_ExceptionShouldBeThrown()
    {
        Func<Task> result = async () => await _handler.Handle(null, CancellationToken.None);

        await result.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task Handle_WhenCalledWithNullManufacturer_ShouldThrowNotFoundException()
    {
        _manufacturerRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Domain.Entities.Manufacturer)null);

        Func<Task> result = async () => await _handler.Handle(new UpdateManufacturerCommand(), CancellationToken.None);

        await result.Should().ThrowAsync<NotFoundException>();
    }
}