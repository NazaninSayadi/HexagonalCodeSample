using Domain.Entities;

namespace Intrastructure.Tests.SampleDataBuilder
{
    
    public class StationBuilder
    {
        public static string StationName => "Station";

        public static ChargeStation WithDefaultValues(Group group)
        {
            return new ChargeStation { Name = StationName ,Group = group,GroupId= group .Id};
        }
    }

}
