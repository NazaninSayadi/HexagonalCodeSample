using Domain.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories
{
    public class ChargeStationRepository : Repository<ChargeStation>, IChargeStationRepository
    {
        private readonly SmartChargingContext _context;
        public ChargeStationRepository(SmartChargingContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ChargeStation>> GetAll()
        {
            return await _context.ChargeStations.Include(x => x.Group).Include(x => x.Connectors).ToListAsync();
        }

        public async Task<ChargeStation?> GetById(Guid id)
        {
            return await _context.ChargeStations.Include(x => x.Group).Include(x => x.Connectors).FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}
