using Domain.Entities;

namespace Intrastructure.Tests.SampleDataBuilder
{

    public class ConnectorBuilder
    {
        public static decimal MaxCurrent => 200;

        public static Connector WithDefaultValues(ChargeStation station)
        {
            return new Connector { MaxCurrent = MaxCurrent, ChargeStation = station, ChargeStationId = station.Id };
        }
    }

}
