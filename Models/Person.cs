using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierSystem.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }

        // Relationship with shipments as navigation properties
        public ICollection<Shipment> SentShipments { get; set; }
        public ICollection<Shipment> ReceivedShipments { get; set; }
    }
}
