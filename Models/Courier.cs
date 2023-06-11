using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierSystem.Models
{
    public class Courier
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Shipment> Shipments { get; set; }
    }
}
