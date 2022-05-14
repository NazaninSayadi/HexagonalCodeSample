using AutoMapper;
using Application.Models;
using Domain.Repositories;
using Application.Services.Interfaces;
using Domain.Entities;

namespace Application.Services.Implementation
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GroupService(IMapper mapper, IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }
       
        public async Task<Guid> Add(string name, decimal capacity)
        {
            if (capacity <= 0)
                throw new ArgumentException("Capacity must be greater than zero.");

            var group = new Group(name, capacity);
            await _groupRepository.Add(group);
            return group.Id;
        }

        public async Task<GroupDTO> Get(Guid Id)
        {
            var group = await _groupRepository.GetById(Id);
            var _mappedGroup = _mapper.Map<GroupDTO>(group);
            return _mappedGroup;
        }

        public async Task<IEnumerable<GroupDTO>> GetAll()
        {
            var groups = await _groupRepository.GetAll();
            var _mappedGroups = _mapper.Map<IEnumerable<GroupDTO>>(groups);
            return _mappedGroups;
        }

        public async Task Remove(Guid id)
        {
            var group = await ValidateGroupExistance(id);
            await _groupRepository.Delete(group);
        }

        public async Task Update(Guid id, string name, decimal capacity)
        {
            var group = await ValidateGroupExistance(id);

            if (group.Capacity != capacity)
                if (GroupCapacityIsValid(capacity, group))
                    group.Capacity = capacity;

            group.Name = name;
            await _groupRepository.Update();
        }

        private static bool GroupCapacityIsValid(decimal capacity, Group group)
        {
            var SumConnectorsMaxCurrent = group.ChargeStations?.SelectMany(x => x.Connectors?.Select(c => c.MaxCurrent)).Sum();

            if (SumConnectorsMaxCurrent != null && capacity < SumConnectorsMaxCurrent)
                throw new ArgumentException("Capacity must always be equal or greater than sum of its connectors max curent");
            return true;
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
