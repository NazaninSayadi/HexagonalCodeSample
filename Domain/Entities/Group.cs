using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Group : Entity
    {
        public Group(string name, decimal capacity)
        {
            Id = Guid.NewGuid();
            Name = name;
            Capacity = capacity;
        }

        public Guid Id { get; private set; }
        public string Name { get; set; }
        public decimal Capacity { get; set; }
        public decimal? SumConnectorsMaxCurrentInAllStation() => ChargeStations?.Select(x => x.SumConnectorsMaxCurrentInEachStation()).Sum();

        public virtual ICollection<ChargeStation> ChargeStations { get; set; }
    }
}
