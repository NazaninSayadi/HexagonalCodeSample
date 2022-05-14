using Domain.Entities;
using Domain.Services.Interfaces;

namespace Domain.Services.Implementations
{
    internal class SmartChargeDomainService : ISmartChargeDomainService
    {
        public bool GroupCapacityIsValid(decimal capacity, Group group)
        {
            var SumConnectorsMaxCurrent = group.ChargeStations?.SelectMany(x => x.Connectors?.Select(c => c.MaxCurrent)).Sum();

            return SumConnectorsMaxCurrent == null || capacity >= SumConnectorsMaxCurrent;
        }

        public bool ValidatePermittedConnectorPerStation(ChargeStation chargeStation) => chargeStation.GetConnectorCount() < 5;


        public bool ValidateRemainingCapacity(decimal MaxCurrent, Group group)
        {
            var remainingCapacity = group.Capacity - group.SumConnectorsMaxCurrentInAllStation();
            return MaxCurrent > remainingCapacity;
        }
    }
}
