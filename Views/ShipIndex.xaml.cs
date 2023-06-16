using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using Microsoft.IdentityModel.Tokens;

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

        private List<ShipmentView> shipmentsViews;
        private List<Shipment> shipments;
        private string selectedShipmentNumber="";
        private Shipment shipmentToChange;
        public ShipIndex()
        {
            InitializeComponent();
            RefreshShipmentListView();
            //ListViewShipment.ItemsSource = shipmentsViews;
        }

        public void RefreshShipmentListView()
        {
            shipmentsViews = new List<ShipmentView>();
            shipments = DB.GetShipmentsWithOtherTables();
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
            ListViewShipment.Items.Refresh();
            DataContext = this;

        }
        private void ListViewShipment_ItemClicked(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = (ListView)sender;
            if (listView.SelectedItem != null)
            {
                ShipmentView selectedShipmentView = (ShipmentView)listView.SelectedItem;

                selectedShipmentNumber = selectedShipmentView.ShipmentNumber;
            }
        }
        
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if(selectedShipmentNumber.IsNullOrEmpty())
            {
                MessageBox.Show("Najpierw wybierz wiersz, który chcesz edytować");
            }
            else {
                shipmentToChange = DB.SearchShipment((long)Convert.ToUInt64(selectedShipmentNumber));
                if (shipmentToChange != null)
                {
                    ShipEdit shipEdit = new ShipEdit(shipmentToChange,this);
                    shipEdit.Show();
                    //RefreshShipmentListView();
                }
                else
                {
                    MessageBox.Show("Coś poszło nie tak, operacja anulowana.");
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedShipmentNumber.IsNullOrEmpty())
            {
                MessageBox.Show("Najpierw wybierz, który chcesz usunąć");
            }
            else
            {
                shipmentToChange = DB.SearchShipment((long)Convert.ToUInt64(selectedShipmentNumber));
                if( shipmentToChange != null)
                {
                    if (MessageBox.Show("Czy na pewno chcesz usunąć zamówienie z nr: " + selectedShipmentNumber + "?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        DB.DeleteShipment(shipmentToChange);
                        MessageBox.Show("Usunięto pomyślnie");
                        RefreshShipmentListView();
                    }
                }
                else{
                    MessageBox.Show("Coś poszło nie tak, operacja anulowana.");
                }

            }

        }
    }
}
