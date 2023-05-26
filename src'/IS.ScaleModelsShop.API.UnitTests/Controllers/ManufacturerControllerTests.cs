using AutoMapper;
using FluentAssertions;
using IS.ScaleModelsShop.API.Contracts.Manufacturer;
using IS.ScaleModelsShop.API.Contracts.Manufacturer.UpdateManufacturer;
using IS.ScaleModelsShop.API.Controllers;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.UpdateManufacturer;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Queries.GetAllManufacturersList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace IS.ScaleModelsShop.API.UnitTests.Controllers
{
    public class ManufacturerControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private Mock<IMapper> _mockMapper;
        private ManufacturerController _controller;
        private UpdateManufacturerCommand _fakeUpdateManufacturerCommand;
        private Guid _fakeManufacturerGuid;

        [SetUp]
        public void Setup()
        {
            _fakeManufacturerGuid = new Guid("00000000-0000-0000-0000-000000000001");

            _mockMediator = new Mock<IMediator>();

            _mockMediator.Setup(x => x.Send(It.IsAny<CreateManufacturerCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ManufacturerModel());
            _mockMediator.Setup(x => x.Send(It.IsAny<UpdateManufacturerCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _fakeUpdateManufacturerCommand = new UpdateManufacturerCommand
            {
                Id = _fakeManufacturerGuid,
                Name = "An updated manufacturer",
                Website = "www.example.org"
            };

            _mockMapper = new Mock<IMapper>();
            _mockMapper.Setup(x => x.Map<UpdateManufacturerCommand>(It.IsAny<UpdateManufacturerModel>()))
                .Returns(_fakeUpdateManufacturerCommand);

            _controller = new ManufacturerController(_mockMediator.Object, _mockMapper.Object);
        }

        #region Constructor

        [Test]
        public void Constructor_WhenCalledWithNullMapper_ShouldThrowArgumentNullException()
        {
            var result = () => new ManufacturerController(_mockMediator.Object, null);

            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Constructor_WhenCalledWithNullMediator_ShouldThrowArgumentNullException()
        {
            var result = () => new ManufacturerController(null, _mockMapper.Object);

            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Constructor_WhenCalledWithValidArguments_ShouldBeInitialized()
        {
            _controller.Should().NotBeNull();
        }

        #endregion

        #region GetAllManufacturers

        [Test]
        public async Task GetAllManufacturers_WhenCalled_ShouldReturnOkObjectResult()
        {
            _mockMediator.Setup(x => x.Send(It.IsAny<GetAllManufacturersListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ManufacturerModel> { new ManufacturerModel() });

            var result = await _controller.GetAllManufacturers();

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
            var model = ((OkObjectResult)result).Value;
            model.Should().NotBeNull();
        }

        [Test]
        public async Task GetAllManufacturers_WhenCalledWithoutEntitiesInStorage_ShouldReturnNoContentResult()
        {
            _mockMediator.Setup(x => x.Send(It.IsAny<GetAllManufacturersListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ManufacturerModel> { });

            var result = await _controller.GetAllManufacturers();

            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        #endregion

        #region CreateManufacturer

        [Test]
        public async Task CreateManufacturer_WhenCalled_ShouldReturnOkObjectResult()
        {
            var result = await _controller.CreateNewManufacturer(new CreateManufacturerCommand());

            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

            var resultOrganizationModel = (ManufacturerModel)((OkObjectResult)result).Value;
            resultOrganizationModel.Should().NotBeNull().And.BeOfType<ManufacturerModel>();
        }

        #endregion

        #region UpdateManufacturer

        [Test]
        public async Task UpdateManufacturer_WhenCalled_ShouldReturnNoContentResult()
        {
            var result = await _controller.UpdateManufacturer(_fakeManufacturerGuid, new UpdateManufacturerModel());

            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        #endregion
    }
}