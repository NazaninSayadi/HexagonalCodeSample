using Domain.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories
{
    public class ConnectorRepository : Repository<Connector>, IConnectorRepository
    {
        private readonly SmartChargingContext _context;
        public ConnectorRepository(SmartChargingContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Connector>> GetAll()
        {
            return await _context.Connectors.Include(x => x.ChargeStation).ThenInclude(x => x.Group).ToListAsync();
        }

        public async Task<Connector?> GetById(int Id, Guid stationId)
        {
            return await _context.Connectors.Include(x => x.ChargeStation).ThenInclude(x => x.Group).FirstOrDefaultAsync(x => x.Id == Id && x.ChargeStationId == stationId);
        }
    }
}
