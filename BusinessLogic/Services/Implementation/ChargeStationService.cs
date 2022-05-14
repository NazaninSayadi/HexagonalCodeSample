using AutoMapper;
using Application.Models;
using Domain.Repositories;
using Application.Services.Interfaces;
using Domain.Entities;

namespace Application.Services.Implementation
{
    public class ChargeStationService : IChargeStationService
    {
        private readonly IChargeStationRepository _chargeStationRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public ChargeStationService(IMapper mapper, IChargeStationRepository chargeStationRepository, IGroupRepository groupRepository)
        {
            _chargeStationRepository = chargeStationRepository;
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Add(string name, Guid groupId)
        {
            var group = await ValidateGroupExistance(groupId);

            var chargeStation = new ChargeStation { Name = name, Group = group, GroupId = groupId };
            await _chargeStationRepository.Add(chargeStation);
            return chargeStation.Id;
        }

        public async Task<ChargeStationDTO> Get(Guid Id)
        {
            var chargeStation = await _chargeStationRepository.GetById(Id);
            var _mappedChargeStation = _mapper.Map<ChargeStationDTO>(chargeStation);
            return _mappedChargeStation;
        }

        public async Task<IEnumerable<ChargeStationDTO>> GetAll()
        {
            var chargeStations = await _chargeStationRepository.GetAll();
            var _mappedChargeStations = _mapper.Map<IEnumerable<ChargeStationDTO>>(chargeStations);
            return _mappedChargeStations;
        }

        public async Task Remove(Guid Id)
        {
            var chargeStation = await ValidateChargeStationExistance(Id);
            await _chargeStationRepository.Delete(chargeStation);
        }

        public async Task Update(Guid id, string name)
        {
            var chargeStation = await ValidateChargeStationExistance(id);

            chargeStation.Name = name;
            await _chargeStationRepository.Update();
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
    }
}
