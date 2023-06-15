using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CourierSystem.Data;
using CourierSystem.Models;

namespace CourierSystem.Views
{
    /// <summary>
    /// Logika interakcji dla klasy ShipIndex.xaml
    /// </summary>
    public partial class ShipIndex : Window
    {
        private class ShipmentView
        {
            public string ShipmentNumber { get; set; }
            public string Status { get; set; }
            public string Courier { get; set; }
            public string Sender { get; set; }
            public string Recipient { get; set; }
            public string Size { get; set; }
        }

        List<ShipmentView> shipmentsViews;
        List<Shipment> shipments;
        public ShipIndex()
        {
            InitializeComponent();
            shipments = DB.GetShipmentsWithOtherTables();
            shipmentsViews = new List<ShipmentView>();
            foreach (var ship in shipments)
            {
                shipmentsViews.Add(
                    new ShipmentView
                    {
                        ShipmentNumber = ship.ShipmentNumber.ToString(),
                        Status = ship.Status.Status,
                        Courier = ship.Courier.Name,
                        Sender = ship.Sender.FirstName + " " + ship.Sender.LastName + ", " + ship.Sender.Address + ", " + ship.Sender.PhoneNumber,
                        Recipient = ship.Recipient.FirstName + " " + ship.Recipient.LastName + ", " + ship.Recipient.Address + ", " + ship.Recipient.PhoneNumber,
                        Size = ship.Size.ToString()
                    });
            }
            ListViewShipment.ItemsSource = shipmentsViews;
        }
    }
}
