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
    public class ChargeStationServiceTests
    {
        private readonly Mock<IGroupRepository> _mockGroupRepository;
        private readonly Mock<IChargeStationRepository> _mockChargeStationRepository;
        private readonly ChargeStationService chargeStationService;

        private Mock<IMapper> _mockMapper;

        public ChargeStationServiceTests()
        {
            _mockGroupRepository = new Mock<IGroupRepository>();
            _mockChargeStationRepository = new Mock<IChargeStationRepository>();

            _mockMapper = new Mock<IMapper>();
            chargeStationService = new ChargeStationService(_mockMapper.Object, _mockChargeStationRepository.Object, _mockGroupRepository.Object);

        }

        [Fact]
        public async void IfInputIsCorrect_AddStation_Successfully()
        {
            //Arrange
            var group = new Group("group", 0);
            _mockGroupRepository.Setup(x => x.GetById(group.Id)).ReturnsAsync(group);
            var chargeStation = new ChargeStation { Name = "Station", GroupId = group.Id, Group = group };

            //Action
            await chargeStationService.Add(chargeStation.Name, chargeStation.GroupId);

            //Assert
            _mockChargeStationRepository.Verify(x => x.Add(It.IsAny<ChargeStation>()), Times.Once);
        }

        [Fact]
        public async void IfGroupDoesntExist_AddStation_ReturnException()
        {
            //Arrange
            var group = new Group("group", 0);
            var chargeStation = new ChargeStation { Name = "Station", GroupId = group.Id, Group = group };

            //ActionAndAssert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
               await chargeStationService.Add(chargeStation.Name, chargeStation.GroupId));
        }

        [Fact]
        public async void GetAllStations()
        {
            //Action
            var chargeStationList = await chargeStationService.GetAll();

            //Assert
            _mockChargeStationRepository.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public async void GetByStationId()
        {
            //Action
            await chargeStationService.Get(new Guid());

            //Assert
            _mockChargeStationRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async void IfStationExist_RemoveStation_Successfully()
        {
            //Arrange
            var group = new Group("group", 1000);
            var chargeStation = new ChargeStation { Name = "Station", GroupId = group.Id, Group = group };

            _mockChargeStationRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(chargeStation);

            //Action
            await chargeStationService.Remove(chargeStation.Id);

            //Assert
            _mockChargeStationRepository.Verify(x => x.Delete(It.IsAny<ChargeStation>()), Times.Once);
        }

        [Fact]
        public async void IfGroupDoesntExist_RemoveGroup_ReturnException()
        {
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await chargeStationService.Remove(new Guid()));
        }

        [Fact]
        public async void IfStationExist_UpdateGroup_Successfully()
        {
            //Arrange
            var group = new Group("group", 1000);
            var chargeStation = new ChargeStation { Name = "Station", GroupId = group.Id, Group = group };
            _mockChargeStationRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(chargeStation);

            //Action
            await chargeStationService.Update(chargeStation.Id, "newName");

            //Assert
            _mockChargeStationRepository.Verify(x => x.Update(), Times.Once);
        }

        [Fact]
        public async void IfStationDoesntExist_UpdateGroup_ReturnException()
        {
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await chargeStationService.Update(new Guid(), "station"));
        }
    }
}