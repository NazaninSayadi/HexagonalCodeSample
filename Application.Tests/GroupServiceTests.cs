using Application.Services.Implementation;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Application.Tests
{
    public class GroupServiceTests
    {
        private readonly Mock<IGroupRepository> _mockGroupRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GroupService groupService;

        public GroupServiceTests()
        {
            _mockGroupRepository = new Mock<IGroupRepository>();
            _mockMapper = new Mock<IMapper>();
            groupService = new GroupService(_mockMapper.Object, _mockGroupRepository.Object);

        }

        [Fact]
        public async void IfInputIsCorrect_AddGroup_Successfully()
        {
            //Arrange
            var group = new Group("group", 1000);

            //Action
            await groupService.Add(group.Name, group.Capacity);

            //Assert
            _mockGroupRepository.Verify(x => x.Add(It.IsAny<Group>()), Times.Once);
        }

        [Fact]
        public async void IfCapacityIsZero_AddGroup_ReturnException()
        {
            //Arrange
            var group = new Group("group", 0);
            _mockGroupRepository.Setup(x => x.Add(group)).ReturnsAsync(group);

            //ActionAndAssert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
               await groupService.Add(group.Name, group.Capacity));
        }

        [Fact]
        public async void GetAllGroup()
        {

            //Action
            var groupList = await groupService.GetAll();

            //Assert
            _mockGroupRepository.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public async void GetByGroupId()
        {
            //Action
            await groupService.Get(new Guid());

            //Assert
            _mockGroupRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async void IfGroupExist_RemoveGroup_Successfully()
        {
            //Arrange
            var group = new Group("group", 1000);
            _mockGroupRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(group);

            //Action
            await groupService.Remove(group.Id);

            //Assert
            _mockGroupRepository.Verify(x => x.Delete(It.IsAny<Group>()), Times.Once);
        }

        [Fact]
        public async void IfGroupDoesntExist_RemoveGroup_ReturnException()
        {
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await groupService.Remove(new Guid()));
        }

        [Fact]
        public async void IfGroupExist_UpdateGroup_Successfully()
        {
            //Arrange
            var group = new Group("group", 1000);
            _mockGroupRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(group);

            //Action
            await groupService.Update(group.Id, "modifiedGroup", 2000);

            //Assert
            _mockGroupRepository.Verify(x => x.Update(), Times.Once);
        }

        [Fact]
        public async void IfGroupDoesntExist_UpdateGroup_ReturnException()
        {
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await groupService.Update(new Guid(), "group", 200));
        }

        [Fact]
        public async void IfSumOfCurrentGroupConnectorsGreaterThanUpdatedCapacity_UpdateGroup_ReturnException()
        {
            //Arrange
            var group = new Group("group", 1000);
            var station = new ChargeStation { Name = "station", GroupId = group.Id, Group = group };
            station.Connectors = new List<Connector>() { new Connector { MaxCurrent = 1000, ChargeStation = station, ChargeStationId = station.Id } };
            group.ChargeStations = new List<ChargeStation>() { station };

            _mockGroupRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(group);

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await groupService.Update(group.Id, group.Name, 500));
        }

    }
}