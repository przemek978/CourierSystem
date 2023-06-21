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
using System.Text.RegularExpressions;
using Org.BouncyCastle.Cms;

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
        private bool isSetSender { get; set; } = false;
        private bool isSetRecipient { get; set; } = false;
        private bool isCheckedRecipient { get; set; } = false;
        private bool isCheckedSender { get; set; } = false;

        private char size { get; set; }
        private string numberPattern = @"^\d{9}$";
        public ShipCreate()
        {
            InitializeComponent();
            couriers = DB.GetCouriers();
            PickCourier.ItemsSource = couriers;
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource =
                new BitmapImage(new Uri("../../../Background.jpg", UriKind.Relative));
            this.Background = myBrush;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                if (!isSetRecipient)
                {
                    DB.AddPerson(recipient);
                }
                if (!isSetSender)
                {
                    DB.AddPerson(this.sender);
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
                AdminPanel adminPanel = new AdminPanel();
                adminPanel.Show();
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel window = new AdminPanel();
            window.Show();
            this.Close();
        }

        private void SearchSenderButton_Click(object sender, RoutedEventArgs e) 
        {

            bool isValidNumber = Regex.IsMatch(SearchSenderNumber.Text, numberPattern);
            if (!isValidNumber)
            {
                MessageBox.Show("Nieprawidłowy numer! (9 cyfr)");
            }
            else
            {
                Person s = DB.SearchPerson(Int32.Parse(SearchSenderNumber.Text));
                if (s == null)
                {
                    MessageBox.Show("Nie znaleziono osoby z numerem " + SearchSenderNumber.Text + ".");
                    isSetSender = false;
                    isCheckedSender = true;
                    SenderAddress.Text = "";
                    SenderFirstName.Text = "";
                    SenderLastName.Text = "";
                    if (FormWindow.Height < 701)
                    {
                        FormWindow.Height = FormWindow.Height + 200;
                    }
                     
                    SenderPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Ustawiono osobę jako nadawcę:\n " + s.FirstName+" "+s.LastName);
                    this.sender = DB.SearchPerson(Int32.Parse(SearchSenderNumber.Text)); 
                    isSetSender = true;
                    isCheckedSender = true;
                    SenderPanel.Visibility = Visibility.Collapsed;
                    if (FormWindow.Height > 300)
                    {
                        FormWindow.Height = FormWindow.Height - 200;
                    }
                    SenderAddress.Text = this.sender.Address;
                    SenderFirstName.Text = this.sender.FirstName;
                    SenderLastName.Text = this.sender.LastName;
                }
            }
        }

        private void SearchRecipientButton_Click(object sender, RoutedEventArgs e) 
        {
            bool isValidNumber = Regex.IsMatch(SearchRecipientNumber.Text, numberPattern);
            if (!isValidNumber)
            {
                MessageBox.Show("Nieprawidłowy numer! (9 cyfr)");
            }
            else
            {
                Person r = DB.SearchPerson(Int32.Parse(SearchRecipientNumber.Text));
                if (r == null)
                {
                    MessageBox.Show("Nie znaleziono osoby z numerem "+ SearchRecipientNumber.Text+".");
                    isSetRecipient = false;
                    isCheckedRecipient = true;
                    RecipientAddress.Text = "";
                    RecipientFirstName.Text = "";
                    RecipientLastName.Text = "";
                    if (FormWindow.Height < 701)
                    {
                        FormWindow.Height = FormWindow.Height + 200;
                    }
                    RecipientPanel.Visibility = Visibility.Visible;
                    
                }
                else
                {
                    MessageBox.Show("Ustawiono osobę jako odbiorcę - " + r.FirstName + " " + r.LastName);
                    recipient = r;
                    isSetRecipient = true;
                    isCheckedRecipient = true;
                    RecipientPanel.Visibility = Visibility.Collapsed;
                    if (FormWindow.Height > 300)
                    {
                        FormWindow.Height = FormWindow.Height - 200;
                    }
                    RecipientAddress.Text = r.Address;
                    RecipientFirstName.Text = r.FirstName;
                    RecipientLastName.Text = r.LastName;

                }
            }
        }

        private void SearchRecipientNumber_TextChanged(object sender, TextChangedEventArgs e) { 
            isCheckedRecipient = false; 
            RecipientPanel.Visibility = Visibility.Collapsed;
            if (FormWindow.Height > 300)
            {
                FormWindow.Height = FormWindow.Height - 200;
            }
        }
        private void SearchSenderNumber_TextChanged(object sender, TextChangedEventArgs e) { 
            isCheckedSender = false; 
            SenderPanel.Visibility = Visibility.Collapsed;
            if (FormWindow.Height > 300)
            {
                FormWindow.Height = FormWindow.Height - 200;
            }
        }

        private bool CheckValidation()
        {
            string message = "";
            bool IsValid = true;

            if (isCheckedRecipient)
            {
                if(!isSetRecipient)
                {
                    recipient = new Person();
                    bool isValidNumber = Regex.IsMatch(SearchRecipientNumber.Text, numberPattern);
                    if (isValidNumber)
                    {
                        recipient.PhoneNumber = Int32.Parse(SearchRecipientNumber.Text);
                    }
                    else
                    {
                        message += "Telefon odbiorcy nie prawidłowy, musi się składać wyłącznie z 9 cyfr\n";
                        IsValid = false;
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
                }
            }
            else { message += "Wyszukaj najpierw numer odbiorcy\n"; IsValid = false; }

            if (isCheckedSender)
            {
                if (!isSetSender)
                {
                    sender = new Person();
                    bool isValidNumber = Regex.IsMatch(SearchSenderNumber.Text, numberPattern);
                    if (isValidNumber)
                    {
                        sender.PhoneNumber = Int32.Parse(SearchSenderNumber.Text);
                    }
                    else
                    {
                        message += "Telefon nadawcy nie prawidłowy, musi się składać wyłącznie z 9 cyfr\n";
                        IsValid = false;
                    }
                    if (SenderFirstName.Text.IsNullOrEmpty())
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
                }
            }
            else { message += "Wyszukaj najpierw numer nadawcy\n"; IsValid = false; }

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
            if(SearchRecipientNumber.Text == SearchSenderNumber.Text)
            {
                message += "Nadawca i odbiorca nie może być ten sam!";
                IsValid = false;
            }
            if(!IsValid)MessageBox.Show(message);
            return IsValid;
        }
    }
}
