using Domain.Entities;
using Infrastructure;
using Infrastructure.Repositories;
using Intrastructure.Tests.SampleDataBuilder;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Intrastructure.Tests
{
    public class GroupRepositoryTests
    {
        private readonly SmartChargingContext _context;
        private readonly GroupRepository _groupRepository;
        private readonly ChargeStationRepository _chargeStationRepository;
        private readonly ConnectorRepository _connectorRepository;
        public GroupRepositoryTests()
        {
            var dbOptions = new DbContextOptionsBuilder<SmartChargingContext>()
                .UseInMemoryDatabase(databaseName: "TestSmartCharging")
                .Options;
            _context = new SmartChargingContext(dbOptions);
            _groupRepository = new GroupRepository(_context);
            _chargeStationRepository = new ChargeStationRepository(_context);
            _connectorRepository = new ConnectorRepository(_context);
        }

        [Fact]
        public async void GetExistGroup()
        {
            //Arrange
            var expectedGroup = GroupBuilder.WithDefaultValues();
            _context.Groups.Add(expectedGroup);
            _context.SaveChanges();

            //Action
            var group = await _groupRepository.GetById(expectedGroup.Id);

            //Assert
            Assert.Equal(expectedGroup.Id, group?.Id);
            Assert.Equal(GroupBuilder.Capacity, group?.Capacity);
            Assert.Equal(GroupBuilder.GroupName, group?.Name);

        }

        [Fact]
        public async void WhenDeleteGroup_AllStationsAndItsConnectorWillBeDeleted()
        {
            //Arrange
            var expectedGroup = GroupBuilder.WithDefaultValues();
            _context.Groups.Add(expectedGroup);

            var station = StationBuilder.WithDefaultValues(expectedGroup);
            var connector = ConnectorBuilder.WithDefaultValues(station);

            station.Connectors = new List<Connector>() { connector };

            _context.ChargeStations.Add(station);
            _context.Connectors.Add(connector);

            _context.SaveChanges();

            //Action
            await _groupRepository.Delete(expectedGroup);

            //Assert
            Assert.Null(await _chargeStationRepository.GetById(station.Id));
            Assert.Null(await _connectorRepository.GetById(connector.Id, connector.ChargeStationId));

        }
    }
}