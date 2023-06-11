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

namespace CourierSystem.Views
{
    /// <summary>
    /// Logika interakcji dla klasy ShipTracking.xaml
    /// </summary>
    public partial class ShipTracking : Window
    {
        public Shipment shipment { get; set; }
        public ShipTracking()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            shipment = DB.SearchShipment(long.Parse(NumberTextBox.Text));

            StatusText.Text = "Status twojej przesyłki";
            if (shipment != null)
            {
                var status = DB.SearchStatus(shipment);
                ResultText.Text = status.Status;
            }
            else
            {
                ResultText.Text = "Przesyłka nie istnieje sprawdź numer przesyłki";
            }
        }
    }
}
