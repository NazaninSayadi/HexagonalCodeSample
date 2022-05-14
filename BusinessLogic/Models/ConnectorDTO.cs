using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class ConnectorDTO
    {
        public int Id { get; set; }
        public Guid ChargeStationId { get; set; }
        public decimal MaxCurrent { get; set; }
    }
}
