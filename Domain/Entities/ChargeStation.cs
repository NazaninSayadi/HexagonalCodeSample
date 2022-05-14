using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ChargeStation: Entity
    {
        public ChargeStation()
        {
            Id= Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public Guid GroupId { get; set; }

        [Required]
        public string Name { get; set; }
        public virtual ICollection<Connector> Connectors { get; set; }
        public Group Group { get; set; }

        public int? GetConnectorCount() => Connectors?.Count;
        public decimal? SumConnectorsMaxCurrentInEachStation() => Connectors?.Select(c=>c.MaxCurrent).Sum();
    }
}
