using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierSystem.Models
{
    public class ShipmentStatus
    {
        public int Id { get; set; }
        public string Status { get; set; }

        public ICollection<Shipment> Shipments { get; set; }
    }
}
