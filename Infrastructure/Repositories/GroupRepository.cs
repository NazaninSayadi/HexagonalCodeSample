using Domain.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        private readonly SmartChargingContext _context;
        public GroupRepository(SmartChargingContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Group>> GetAll()
        {
            return await _context.Groups.Include(x => x.ChargeStations).ThenInclude(x => x.Connectors).ToListAsync();
        }

        public async Task<Group?> GetById(Guid groupId)
        {
            return await _context.Groups.Include(x => x.ChargeStations).ThenInclude(x => x.Connectors).FirstOrDefaultAsync(x => x.Id == groupId);
        }

    }
}
