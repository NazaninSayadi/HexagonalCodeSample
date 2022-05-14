using Domain.Entities;
using Domain.Repositories.Base;

namespace Domain.Repositories
{
    public interface IChargeStationRepository : IRepository<ChargeStation>
    {
        Task<ChargeStation> GetById(Guid id);
        Task<IReadOnlyList<ChargeStation>> GetAll();
    }
}
