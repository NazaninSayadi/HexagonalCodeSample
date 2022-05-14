using Application.Models;

namespace Application.Services.Interfaces
{
    public interface IConnectorService
    {
        Task<IEnumerable<ConnectorDTO>> GetAll();
        Task<ConnectorDTO> Get(int id,Guid stationId);
        Task<int> Add(decimal MaxCurrent, Guid stationId);
        Task Update(int id, Guid stationId, decimal MaxCurrent);
        Task Remove(int id, Guid stationId);

    }
}
