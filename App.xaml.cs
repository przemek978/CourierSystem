using CourierSystem.Data;
using CourierSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CourierSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var context = DB.GetInstance();


            if (context.Couriers.Any() && context.People.Any() && context.Statuses.Any() && !context.Shipments.Any())
            {
                for (int i = 0; i < 3; i++)
                {
                    char[] sizes = { 'A', 'B', 'C' };
                    var courier = context.Couriers.ToList()[i].Id;
                    var sender = context.People.ToList()[i].Id;
                    var recipient = context.People.ToList()[(i + 1) % 3].Id;
                    long trackingNumber;
                    while (true)
                    {
                        trackingNumber = Shipment.GenerateTrackingNumber();
                        if (context.Shipments.Any(s => s.ShipmentNumber == trackingNumber))
                        {
                            continue;
                        }
                        break;
                    }
                    context.Shipments.Add(new Shipment { ShipmentNumber = trackingNumber, CourierID = courier, SenderID = sender, RecipientID = recipient, Size = sizes[i], StatusId = context.Statuses.ToList()[0].Id });
                }
                context.SaveChanges();
            }

        }
    }
}
