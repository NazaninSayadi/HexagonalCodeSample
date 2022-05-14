using Application.Services.Implementation;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Application.Tests
{
    public class ConnectorServiceTests
    {
        private readonly Mock<IGroupRepository> _mockGroupRepository;
        private readonly Mock<IChargeStationRepository> _mockChargeStationRepository;
        private readonly Mock<IConnectorRepository> _mockConnectorRepository;
        private readonly ConnectorService connectorService;


        private Mock<IMapper> _mockMapper;

        public ConnectorServiceTests()
        {
            _mockGroupRepository = new Mock<IGroupRepository>();
            _mockChargeStationRepository = new Mock<IChargeStationRepository>();
            _mockConnectorRepository = new Mock<IConnectorRepository>();
            _mockMapper = new Mock<IMapper>();

            connectorService = new ConnectorService(_mockMapper.Object, _mockConnectorRepository.Object, _mockChargeStationRepository.Object, _mockGroupRepository.Object);

        }

        [Fact]
        public async void IfInputIsCorrect_AddConnector_Successfully()
        {
            //Arrange
            var group = new Group("group", 1000);
            var station = new ChargeStation { Name = "station", GroupId = group.Id, Group = group };
            group.ChargeStations = new List<ChargeStation>() { station };

            var connector = new Connector { MaxCurrent = 1000, ChargeStation = station, ChargeStationId = station.Id };

            _mockChargeStationRepository.Setup(x => x.GetById(station.Id)).ReturnsAsync(station);
            _mockGroupRepository.Setup(x => x.GetById(group.Id)).ReturnsAsync(group);


            //Action
            await connectorService.Add(connector.MaxCurrent, station.Id);

            //Assert
            _mockConnectorRepository.Verify(x => x.Add(It.IsAny<Connector>()), Times.Once);
        }

        [Fact]
        public async void IfChargeStationDoesntExist_AddConnector_ReturnException()
        {
            //Arrange
            var group = new Group("group", 1000);
            var station = new ChargeStation { Name = "station", GroupId = group.Id, Group = group };
            group.ChargeStations = new List<ChargeStation>() { station };

            var connector = new Connector { MaxCurrent = 1000, ChargeStation = station, ChargeStationId = station.Id };

            //ActionAndAssert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
               await connectorService.Add(connector.MaxCurrent, connector.ChargeStationId));
        }
        [Fact]
        public async void IfGroupDoesntExist_AddConnector_ReturnException()
        {
            //Arrange
            var station = new ChargeStation { Name = "station" };
            _mockChargeStationRepository.Setup(x => x.GetById(station.Id)).ReturnsAsync(station);


            var connector = new Connector { MaxCurrent = 1000, ChargeStation = station, ChargeStationId = station.Id };

            //ActionAndAssert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
               await connectorService.Add(connector.MaxCurrent, connector.ChargeStationId));
        }
        [Fact]
        public async void IfCountOfPermittedConnectorExceeded_AddConnector_ReturnException()
        {
            //Arrange
            var group = new Group("group", 1000);
            var station = new ChargeStation { Name = "station", GroupId = group.Id, Group = group };

            station.Connectors = new List<Connector>() {
                new Connector { MaxCurrent = 100, ChargeStation = station, ChargeStationId = station.Id },
                new Connector { MaxCurrent = 100, ChargeStation = station, ChargeStationId = station.Id },
                new Connector { MaxCurrent = 100, ChargeStation = station, ChargeStationId = station.Id },
                new Connector { MaxCurrent = 100, ChargeStation = station, ChargeStationId = station.Id },
                new Connector { MaxCurrent = 100, ChargeStation = station, ChargeStationId = station.Id }
            };
            group.ChargeStations = new List<ChargeStation>() { station };
            var connector = new Connector { MaxCurrent = 100, ChargeStation = station, ChargeStationId = station.Id };

            _mockChargeStationRepository.Setup(x => x.GetById(station.Id)).ReturnsAsync(station);
            _mockGroupRepository.Setup(x => x.GetById(group.Id)).ReturnsAsync(group);

            //ActionAndAssert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
               await connectorService.Add(connector.MaxCurrent, connector.ChargeStationId));
        }

        [Fact]
        public async void IfSumOfCurrentBeGreaterThanGroupCapacity_AddConnector_ReturnException()
        {
            //Arrange
            var group = new Group("group", 1000);
            var station = new ChargeStation { Name = "station", GroupId = group.Id, Group = group };
            station.Connectors = new List<Connector>() { new Connector { MaxCurrent = 800, ChargeStation = station, ChargeStationId = station.Id } };
            group.ChargeStations = new List<ChargeStation>() { station };

            var connector = new Connector { MaxCurrent = 300, ChargeStation = station, ChargeStationId = station.Id };

            _mockChargeStationRepository.Setup(x => x.GetById(station.Id)).ReturnsAsync(station);
            _mockGroupRepository.Setup(x => x.GetById(group.Id)).ReturnsAsync(group);

            //ActionAndAssert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
               await connectorService.Add(connector.MaxCurrent, connector.ChargeStationId));
        }

        [Fact]
        public async void GetAllConnector()
        {
            //Action
            var chargeStationList = await connectorService.GetAll();

            //Assert
            _mockConnectorRepository.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public async void GetById()
        {
            //Action
            await connectorService.Get(1, new Guid());

            //Assert
            _mockConnectorRepository.Verify(x => x.GetById(It.IsAny<int>(), It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async void IfConnectorExist_RemoveConnector_Successfully()
        {
            //Arrange
            var connector = new Connector { MaxCurrent = 200 };

            _mockConnectorRepository.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<Guid>())).ReturnsAsync(connector);

            //Action
            await connectorService.Remove(1, new Guid());

            //Assert
            _mockConnectorRepository.Verify(x => x.Delete(It.IsAny<Connector>()), Times.Once);
        }

        [Fact]
        public async void IfConnectorDoesntExist_RemoveConnector_ReturnException()
        {
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
               await connectorService.Remove(1, new Guid()));
        }

        [Fact]
        public async void IfInputIsValid_UpdateConnector_Successfully()
        {
            //Arrange
            var group = new Group("group", 1000);
            var station = new ChargeStation { Name = "station", GroupId = group.Id, Group = group };
            var connector = new Connector { MaxCurrent = 200, ChargeStation = station, ChargeStationId = station.Id };
            station.Connectors = new List<Connector>() { connector };
            group.ChargeStations = new List<ChargeStation>() { station };


            _mockConnectorRepository.Setup(x => x.GetById(connector.Id,station.Id)).ReturnsAsync(connector);
            _mockGroupRepository.Setup(x => x.GetById(group.Id)).ReturnsAsync(group);

            //Action
            await connectorService.Update(connector.Id, connector.ChargeStationId, 300);

            //Assert
            _mockConnectorRepository.Verify(x => x.Update(), Times.Once);
        }

        [Fact]
        public async void IfConnectorDoesntExist_UpdateConnector_ReturnException()
        {
            //Arrange
            var connector = new Connector { MaxCurrent = 1000, ChargeStationId = new Guid() };

            //ActionAndAssert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
               await connectorService.Update(connector.Id, connector.ChargeStationId, connector.MaxCurrent));
        }

        [Fact]
        public async void IfGroupDoesntExist_UpdateConnector_ReturnException()
        {
            //Arrange
            var group = new Group("group", 1000);
            var station = new ChargeStation { Name = "station", GroupId = group.Id, Group = group };
            station.Connectors = new List<Connector>() { new Connector { MaxCurrent = 800, ChargeStation = station, ChargeStationId = station.Id } };
            group.ChargeStations = new List<ChargeStation>() { station };
            var connector = new Connector { MaxCurrent = 300, ChargeStation = station, ChargeStationId = station.Id };
            _mockConnectorRepository.Setup(x => x.GetById(connector.Id, connector.ChargeStationId)).ReturnsAsync(connector);

            //ActionAndAssert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
               await connectorService.Update(connector.Id, connector.ChargeStationId, connector.MaxCurrent));
        }

        [Fact]
        public async void IfSumOfCurrentBeGreaterThanGroupCapacity_UpdateConnector_ReturnException()
        {
            //Arrange
            var group = new Group("group", 1000);
            var station = new ChargeStation { Name = "station", GroupId = group.Id, Group = group };
            station.Connectors = new List<Connector>() { new Connector { MaxCurrent = 800, ChargeStation = station, ChargeStationId = station.Id } };
            group.ChargeStations = new List<ChargeStation>() { station };

            var connector = new Connector { MaxCurrent = 300, ChargeStation = station, ChargeStationId = station.Id };

            _mockChargeStationRepository.Setup(x => x.GetById(station.Id)).ReturnsAsync(station);
            _mockGroupRepository.Setup(x => x.GetById(group.Id)).ReturnsAsync(group);

            //ActionAndAssert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
               await connectorService.Add(connector.MaxCurrent, connector.ChargeStationId));
        }

    }
}