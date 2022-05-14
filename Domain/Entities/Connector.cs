using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Connector : Entity
    {
        public int Id { get; set; }
        public Guid ChargeStationId { get; set; }

        [Required]
        public decimal MaxCurrent { get; set; }

        public ChargeStation ChargeStation { get; set; }

    }
}
