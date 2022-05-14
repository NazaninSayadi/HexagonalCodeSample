namespace ApiGateway.Models.Connector
{
    public class CreateConnectorInputModel
    {
        public decimal MaxCurrent { get; set; }
        public Guid ChargeStationId { get; set; }
    }
}
