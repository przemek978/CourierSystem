using CourierSystem.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int StatusId { get; set; }
        public char Size { get; set; }

        public ShipmentStatus Status { get; set; }
        public Courier Courier { get; set; }
        public Person Sender { get; set; }
        public Person Recipient { get; set; }

        public static long GenerateTrackingNumber()
        {
            Random random = new Random();
            const long companyNumber = 705;
            const int maxValue = 99999999;

            int randomIntValue = random.Next(maxValue);
            long randomValue = (long)randomIntValue;

            long trackingNumber = companyNumber * 100000000 + randomValue;

            return trackingNumber;
        }
    }
}
