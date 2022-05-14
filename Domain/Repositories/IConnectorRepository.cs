using Domain.Entities;
using Domain.Repositories.Base;

namespace Domain.Repositories
{
    public interface IConnectorRepository :IRepository<Connector>
    {
        Task<Connector> GetById(int Id,Guid stationId);
        Task<IReadOnlyList<Connector>> GetAll();
    }
}
