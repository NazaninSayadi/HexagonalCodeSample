using Application.Models;

namespace Application.Services.Interfaces
{
    public interface IChargeStationService
    {
        Task<IEnumerable<ChargeStationDTO>> GetAll();
        Task<ChargeStationDTO> Get(Guid Id);
        Task<Guid> Add(string chargeStationName, Guid groupId);
        Task Update(Guid Id,string name);
        Task Remove(Guid Id);

    }
}
