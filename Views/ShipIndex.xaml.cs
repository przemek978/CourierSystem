using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CourierSystem.Data;
using CourierSystem.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.IdentityModel.Tokens;
using Document = iTextSharp.text.Document;

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
        private List<ShipmentStatus> statuses;
        private string selectedShipmentNumber = "";
        private Shipment shipmentToChange;
        private Models.User user;

        public ShipIndex(User user)
        {
            this.user = user;
            InitializeComponent();
            RefreshShipmentListView();
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource =
                new BitmapImage(new Uri("../../../Background.jpg", UriKind.Relative));
            this.Background = myBrush;
            //ListViewShipment.ItemsSource = shipmentsViews;
        }

        public void RefreshShipmentListView()
        {
            shipmentsViews = new List<ShipmentView>();
            shipments = DB.GetShipmentsWithOtherTables();
            statuses = DB.GetStatuses();
            foreach (var ship in shipments)
            {
                shipmentsViews.Add(
                    new ShipmentView
                    {
                        ShipmentNumber = ship.ShipmentNumber.ToString(),
                        Status = ship.Status.Status,
                        Courier = ship.Courier.Name,
                        Sender = ship.Sender.FirstName + " " + ship.Sender.LastName + ", " + ship.Sender.Address +
                                 ", " + ship.Sender.PhoneNumber,
                        Recipient = ship.Recipient.FirstName + " " + ship.Recipient.LastName + ", " +
                                    ship.Recipient.Address + ", " + ship.Recipient.PhoneNumber,
                        Size = ship.Size.ToString()
                    });
            }

            ListViewShipment.ItemsSource = shipmentsViews;
            StatusCombo.ItemsSource = statuses;
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
                var ship = DB.SearchShipment((long)Convert.ToUInt64(selectedShipmentNumber));
                StatusCombo.SelectedIndex = StatusCombo.Items.Cast<ShipmentStatus>().ToList()
                    .FindIndex(item => item.Status == ship.Status.Status);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedShipmentNumber.IsNullOrEmpty())
            {
                MessageBox.Show("Najpierw wybierz wiersz, który chcesz edytować");
            }
            else
            {
                shipmentToChange = DB.SearchShipment((long)Convert.ToUInt64(selectedShipmentNumber));
                if (shipmentToChange != null)
                {
                    ShipEdit shipEdit = new ShipEdit(shipmentToChange, this);
                    shipEdit.Show();
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
                if (shipmentToChange != null)
                {
                    if (MessageBox.Show("Czy na pewno chcesz usunąć zamówienie z nr: " + selectedShipmentNumber + "?",
                            "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        DB.DeleteShipment(shipmentToChange);
                        MessageBox.Show("Usunięto pomyślnie");
                        RefreshShipmentListView();
                    }
                }
                else
                {
                    MessageBox.Show("Coś poszło nie tak, operacja anulowana.");
                }

            }
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
                    RefreshShipmentListView();
                }
                else
                {
                    MessageBox.Show("Coś poszło nie tak, operacja anulowana.");
                }

            }
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel window = new AdminPanel();
            window.Show();
            this.Close();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            Printer.GenerujListPrzewozowy(shipments,user);
        }
    }
}

