using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierSystem.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public long ShipmentNumber { get; set; }
        public Shipment Shipment { get; set; }

        public Message(string content, long shipmentNumber)
        {
            Content = content;
            ShipmentNumber = shipmentNumber;
        }
    }
}
