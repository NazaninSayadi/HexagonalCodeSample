namespace Application.Models
{
    public class ChargeStationDTO
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public IEnumerable<ConnectorDTO> Connectors { get; set; }
    }
}
