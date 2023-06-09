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

            using (var context = DB.GetInstance())
            {
                if (context.Couriers.Any() && context.People.Any() && !context.Shipments.Any())
                {
                    for (int i = 0; i < 3; i++)
                    {
                        char[] sizes = { 'A', 'B', 'C' };
                        var courier = context.Couriers.ToList()[i].Id;
                        var sender = context.People.ToList()[i].Id;
                        var recipient = context.People.ToList()[(i + 1) % 3].Id;

                        context.Shipments.Add(new Shipment { CourierID = courier, SenderID = sender, RecipientID = recipient, Size = sizes[i] });
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
