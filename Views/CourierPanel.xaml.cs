using CourierSystem.Data;
using CourierSystem.Models;
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
using Microsoft.IdentityModel.Tokens;

namespace CourierSystem.Views
{
    /// <summary>
    /// Logika interakcji dla klasy CourierPanel.xaml
    /// </summary>
    public partial class CourierPanel : Window
    {
        private class ShipmentCourierView
        {
            public string ShipmentNumber { get; set; }
            public string Status { get; set; }
            public string Sender { get; set; }
            public string Recipient { get; set; }
            public string Size { get; set; }
        }

        private List<ShipmentCourierView> shipmentsViews;
        private List<Shipment> shipments;
        private List<ShipmentStatus> statuses;
        private string selectedShipmentNumber = "";
        private Shipment shipmentToChange;
        private Models.User user;
        public CourierPanel(Models.User user)
        {
            InitializeComponent();
            this.user = user;
            RefreshShipmentListView("");
        }

        private void RefreshShipmentListView(String contains)
        {
            shipmentsViews = new List<ShipmentCourierView>();
            shipments = DB.GetShipmentsWithOtherTables();
            statuses = DB.GetStatuses();
            foreach (var ship in shipments)
            {
                if (ship.CourierID == user.Id && ship.ShipmentNumber.ToString().Contains(contains))
                {
                    shipmentsViews.Add(
                        new ShipmentCourierView
                        {
                            ShipmentNumber = ship.ShipmentNumber.ToString(),
                            Status = ship.Status.Status,
                            Sender = ship.Sender.FirstName + " " + ship.Sender.LastName + ", " + ship.Sender.Address + ", " + ship.Sender.PhoneNumber,
                            Recipient = ship.Recipient.FirstName + " " + ship.Recipient.LastName + ", " + ship.Recipient.Address + ", " + ship.Recipient.PhoneNumber,
                            Size = ship.Size.ToString()
                        });
                }
            }
            ListViewCourierShipment.ItemsSource = shipmentsViews;
            StatusCombo.ItemsSource = statuses;
            ListViewCourierShipment.Items.Refresh();
            DataContext = this;
        }

        private void ListViewShipment_ItemClicked(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = (ListView)sender;
            if (listView.SelectedItem != null)
            {
                ShipmentCourierView selectedShipmentView = (ShipmentCourierView)listView.SelectedItem;
                selectedShipmentNumber = selectedShipmentView.ShipmentNumber;
                var ship = DB.SearchShipment((long)Convert.ToUInt64(selectedShipmentNumber));
                StatusCombo.SelectedIndex = StatusCombo.Items.Cast<ShipmentStatus>().ToList().FindIndex(item => item.Status == ship.Status.Status);
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            String text = SearchCourierShipment.Text;
            RefreshShipmentListView(text);
        }

        private void StatusSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (selectedShipmentNumber.IsNullOrEmpty())
            {
                MessageBox.Show("Najpierw wybierz element, ktoremu chcesz zmienić status");
            }
            else
            {
                shipmentToChange = DB.SearchShipment((long)Convert.ToUInt64(selectedShipmentNumber));
                if (shipmentToChange != null)
                {
                    shipmentToChange.Status = (ShipmentStatus)StatusCombo.SelectedItem;
                    DB.EditShipment(shipmentToChange);
                    //MessageBox.Show("Zmieniono status");
                    RefreshShipmentListView("");
                }
                else
                {
                    MessageBox.Show("Coś poszło nie tak, operacja anulowana.");
                }

            }
        }
    }
    
}
