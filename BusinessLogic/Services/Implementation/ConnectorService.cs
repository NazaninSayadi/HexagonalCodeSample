using AutoMapper;
using Application.Models;
using Domain.Repositories;
using Application.Services.Interfaces;
using Domain.Entities;

namespace Application.Services.Implementation
{
    public class ConnectorService : IConnectorService
    {
        private readonly IChargeStationRepository _chargeStationRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IConnectorRepository _connectorRepository;

        private readonly IMapper _mapper;

        public ConnectorService(IMapper mapper, IConnectorRepository connectorRepository, IChargeStationRepository chargeStationRepository, IGroupRepository groupRepository)
        {
            _chargeStationRepository = chargeStationRepository;
            _connectorRepository = connectorRepository;
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public async Task<int> Add(decimal MaxCurrent, Guid stationId)
        {
            var chargeStation = await ValidateChargeStationExistance(stationId);

            ValidatePermittedConnectorPerStation(chargeStation);

            var group = await ValidateGroupExistance(chargeStation.GroupId);

            ValidateRemainingCapacity(MaxCurrent, group);

            var connector = new Connector
            {
                Id = chargeStation.GetConnectorCount() ?? +1,
                MaxCurrent = MaxCurrent,
                ChargeStationId = stationId,
                ChargeStation = chargeStation,
            };

            await _connectorRepository.Add(connector);
            return connector.Id;
        }

        public async Task<ConnectorDTO> Get(int id, Guid stationId)
        {
            var connectors = await _connectorRepository.GetById(id, stationId);
            var _mappedConnector = _mapper.Map<ConnectorDTO>(connectors);
            return _mappedConnector;
        }

        public async Task<IEnumerable<ConnectorDTO>> GetAll()
        {
            var connectors = await _connectorRepository.GetAll();
            var _mappedConnectors = _mapper.Map<IEnumerable<ConnectorDTO>>(connectors);
            return _mappedConnectors;
        }

        public async Task Remove(int id, Guid stationId)
        {
            var connector = await ValidateConnectorExistance(id, stationId);
            await _connectorRepository.Delete(connector);
        }

        public async Task Update(int id, Guid stationId, decimal MaxCurrent)
        {
            var connector = await ValidateConnectorExistance(id, stationId);
            var group = await ValidateGroupExistance(connector.ChargeStation.GroupId);

            ValidateRemainingCapacity(MaxCurrent, group);

            connector.MaxCurrent = MaxCurrent;
            await _connectorRepository.Update();
        }

        #region privateMethods
        private async Task<Connector> ValidateConnectorExistance(int id, Guid stationId)
        {
            var connector = await _connectorRepository.GetById(id, stationId);
            if (connector == null)
                throw new ArgumentException($"There is no connector with this Id: {id} for this Station {stationId}");

            return connector;
        }

        private async Task<ChargeStation> ValidateChargeStationExistance(Guid id)
        {
            var chargeStation = await _chargeStationRepository.GetById(id);
            if (chargeStation == null)
                throw new ArgumentException($"There is no chargeStation with this Id: {id}");

            return chargeStation;
        }

        private static void ValidatePermittedConnectorPerStation(ChargeStation chargeStation)
        {
            if (chargeStation.GetConnectorCount() == 5) //ToDo read from config
                throw new ArgumentException("We have reached the quorum of connectors.");
        }

        private async Task<Group> ValidateGroupExistance(Guid id)
        {
            var group = await _groupRepository.GetById(id);
            if (group == null)
                throw new ArgumentException($"There is no group with this Id: {id}");

            return group;
        }

        private static void ValidateRemainingCapacity(decimal MaxCurrent, Group group)
        {
            var remainingCapacity = group.Capacity - group.SumConnectorsMaxCurrentInAllStation();
            if (MaxCurrent > remainingCapacity)
                throw new ArgumentException("There is no capacity for this connector");
        }
        #endregion
    }
}
