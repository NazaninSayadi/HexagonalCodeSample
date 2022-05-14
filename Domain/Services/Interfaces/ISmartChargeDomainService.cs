using Domain.Entities;

namespace Domain.Services.Interfaces
{
    public interface ISmartChargeDomainService
    {
        bool ValidatePermittedConnectorPerStation(ChargeStation chargeStation);
        bool ValidateRemainingCapacity(decimal MaxCurrent, Group group);
        bool GroupCapacityIsValid(decimal capacity, Group group);
    }
}
