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
using CourierSystem.Data;
using Microsoft.IdentityModel.Tokens;

namespace CourierSystem.Views
{
    /// <summary>
    /// Logika interakcji dla klasy ShipCreate.xaml
    /// </summary>
    public partial class ShipCreate : Window
    {
        private List<Courier> couriers;
        private Person recipient { get; set; }
        private Person sender { get; set; }
        private Courier courier { get; set; }
        private char size { get; set; }
        public ShipCreate()
        {
            InitializeComponent();
            couriers = DB.GetCouriers();
            PickCourier.ItemsSource = couriers;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                Person r = DB.SearchPerson(recipient.PhoneNumber);
                if (r == null)
                {
                    DB.AddPerson(recipient);
                }
                else
                {
                    recipient = r;
                }
                Person s = DB.SearchPerson(this.sender.PhoneNumber);
                if (s == null)
                {
                    DB.AddPerson(this.sender);
                }
                else
                {
                    this.sender = s;
                }
                Shipment shipment = new Shipment
                {
                    ShipmentNumber = Shipment.GenerateTrackingNumber(),
                    CourierID = courier.Id,
                    RecipientID = recipient.Id,
                    SenderID = this.sender.Id,
                    StatusId = 1,
                    Size = size
                };
                DB.AddShipment(shipment);
                MessageBox.Show("Dane o przesyłce przyjęte");
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool CheckValidation()
        {
            recipient = new Person(); 
            sender = new Person(); 
            string message = "";
            int parsedNumber;
            bool IsValid = true;
            if (int.TryParse(RecipientNumber.Text, out parsedNumber) && RecipientNumber.Text.Length == 9)
            {
                recipient.PhoneNumber = parsedNumber;
            }
            else
            {
                message += "Telefon odbiorcy nie prawidłowy, musi się składać wyłącznie z 9 cyfr\n";
                IsValid = false;
            }
            if (int.TryParse(SenderNumber.Text, out parsedNumber) && SenderNumber.Text.Length == 9)
            {
                sender.PhoneNumber = parsedNumber;
            }
            else
            {
                message += "Telefon nadawcy nie prawidłowy, musi się składać wyłącznie z 9 cyfr\n";
                IsValid = false;
            }
            if (SenderFirstName.Text.IsNullOrEmpty() )
            {
                message += "Pole imię nadawcy jest wymagane\n";
                IsValid = false;
            }
            else
            {
                sender.FirstName = SenderFirstName.Text;
            }
            if (SenderLastName.Text.IsNullOrEmpty())
            {
                message += "Pole nazwisko nadawcy jest wymagane\n";
                IsValid = false;
            }
            else
            {
                sender.LastName = SenderLastName.Text;
            }
            if (SenderAddress.Text.IsNullOrEmpty())
            {
                message += "Pole adres nadawcy jest wymagane\n";
                IsValid = false;
            }
            else
            {
                sender.Address = SenderAddress.Text;
            }
            if (RecipientFirstName.Text.IsNullOrEmpty())
            {
                message += "Pole imię odbiorcy jest wymagane\n";
                IsValid = false;
            }
            else
            {
                recipient.FirstName = RecipientFirstName.Text;
            }
            if (RecipientLastName.Text.IsNullOrEmpty())
            {
                message += "Pole nazwisko odbiorcy jest wymagane\n";
                IsValid = false;
            }
            else
            {
                recipient.LastName = RecipientLastName.Text;
            }
            if (RecipientAddress.Text.IsNullOrEmpty())
            {
                message += "Pole adres odbiorcy jest wymagane\n";
                IsValid = false;
            }
            else
            {
                recipient.Address = RecipientAddress.Text;
            }
            if (PickCourier.SelectedIndex>=0)
            {
                courier = couriers.FirstOrDefault(c => c.Id == PickCourier.SelectedIndex+1);
            }
            else
            {
                message += "Przydziel kuriera\n";
                IsValid = false;
            }
            if (size0.IsChecked == true)
            {
                size = 'A';
            }
            else if (size1.IsChecked == true)
            {
                size = 'B';
            }
            else if (size2.IsChecked == true)
            {
                size = 'C';
            }
            else
            {
                IsValid = false;
                message += "Wybierz rozmiar\n";

            }
            if(!IsValid)MessageBox.Show(message);
            return IsValid;
        }
    }
}
