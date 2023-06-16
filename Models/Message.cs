using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CourierSystem.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserType { get; set; }
        public long ShipmentNumber { get; set; }
        public bool Status { get; set; }
        public Shipment Shipment { get; set; }

        public Message(string content, long shipmentNumber,string userType, bool status=false)
        {
            Content = content;
            ShipmentNumber = shipmentNumber;
            UserType = userType;
            Status = status;
        }
    }
}
