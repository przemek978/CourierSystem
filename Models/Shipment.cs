using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierSystem.Models
{
    public class Shipment
    {
        [Key]
        public long ShipmentNumber { get; set; }

        public int SenderID { get; set; }
        public int RecipientID { get; set; }
        public int CourierID { get; set; }
        public char Size { get; set; }

        // Relationships with other entities as navigation properties
        public Courier Courier { get; set; }
        public Person Sender { get; set; }
        public Person Recipient { get; set; }
    }
}
