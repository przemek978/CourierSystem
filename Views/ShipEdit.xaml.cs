using CourierSystem.Data;
using CourierSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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

namespace CourierSystem.Views
{
    /// <summary>
    /// Logika interakcji dla klasy ShipEdit.xaml
    /// </summary>
    public partial class ShipEdit : Window
    {
        Shipment shipmentToChange;
        private List<Courier> couriers;
        private List<ShipmentStatus> statuses;
        private Window window;
        public ShipEdit(Shipment shipment, Window window)
        {
            InitializeComponent();
            this.shipmentToChange = shipment;
            couriers = DB.GetCouriers();
            PickCourier.ItemsSource = couriers;
            statuses = DB.GetStatuses();
            PickStatus.ItemsSource = statuses;
            CourierHeader.Header = "Zmień kuriera z "+couriers.ElementAt(shipment.CourierID-1).Name+" na:";
            StatusHeader.Header = "Zmień status z "+statuses.ElementAt(shipment.StatusId-1).Status+" na:";
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource =
                new BitmapImage(new Uri("../../../Background.jpg", UriKind.Relative));
            this.Background = myBrush;
            this.window = window;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (PickCourier.SelectedIndex >= 0)
            {
                shipmentToChange.CourierID = couriers.FirstOrDefault(c => c.Id == PickCourier.SelectedIndex + 1).Id;
            }
            if (PickStatus.SelectedIndex >= 0)
            {
                shipmentToChange.StatusId = statuses.FirstOrDefault(c => c.Id == PickStatus.SelectedIndex + 1).Id;
            }
            DB.EditShipment(shipmentToChange);
            ((ShipIndex)window).RefreshShipmentListView();
            MessageBox.Show("Zaktualizowano pomyślnie");
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
