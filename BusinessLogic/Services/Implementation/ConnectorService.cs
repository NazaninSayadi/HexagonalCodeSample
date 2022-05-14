using AutoMapper;
using Application.Models;
using Domain.Repositories;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Services.Interfaces;

namespace Application.Services.Implementation
{
    public class ConnectorService : IConnectorService
    {
        private readonly IChargeStationRepository _chargeStationRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IConnectorRepository _connectorRepository;
        private readonly ISmartChargeDomainService _smartChargeDomainService;


        private readonly IMapper _mapper;

        public ConnectorService(IMapper mapper, IConnectorRepository connectorRepository, IChargeStationRepository chargeStationRepository, IGroupRepository groupRepository, ISmartChargeDomainService smartChargeDomainService)
        {
            _chargeStationRepository = chargeStationRepository;
            _connectorRepository = connectorRepository;
            _groupRepository = groupRepository;
            _smartChargeDomainService = smartChargeDomainService;
            _mapper = mapper;
        }

        public async Task<int> Add(decimal MaxCurrent, Guid stationId)
        {
            var chargeStation = await ValidateChargeStationExistance(stationId);

            if (!_smartChargeDomainService.ValidatePermittedConnectorPerStation(chargeStation))
                throw new ArgumentException("We have reached the quorum of connectors.");

            var group = await ValidateGroupExistance(chargeStation.GroupId);

            if (!_smartChargeDomainService.ValidateRemainingCapacity(MaxCurrent, group))
                throw new ArgumentException("There is no capacity for this connector");

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

            if (!_smartChargeDomainService.ValidateRemainingCapacity(MaxCurrent, group))
                throw new ArgumentException("There is no capacity for this connector");


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

        private async Task<Group> ValidateGroupExistance(Guid id)
        {
            var group = await _groupRepository.GetById(id);
            if (group == null)
                throw new ArgumentException($"There is no group with this Id: {id}");

            return group;
        }
        #endregion
    }
}
