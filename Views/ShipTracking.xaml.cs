﻿using CourierSystem.Data;
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
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource =
                new BitmapImage(new Uri("../../../Background.jpg", UriKind.Relative));
            this.Background = myBrush;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            long number;
            long.TryParse(NumberTextBox.Text, out number);
            shipment = DB.SearchShipment(number);

            StatusText.Text = "Status twojej przesyłki";
            if (shipment != null)
            {
                var status = DB.SearchStatus(shipment);
                ResultText.Text = status.Status;
                InfoPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Przesyłka nie istnieje sprawdź numer przesyłki");
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageText.Text != "")
            {
                var message = new Message(MessageText.Text, shipment.ShipmentNumber,UserTypeCombo.SelectionBoxItem.ToString());
                DB.SendMessage(message);
                MessageBox.Show("Wiadomość została wysłana");
                MessageText.Text = "";
            }
            else
            {
                MessageBox.Show("Pole z wiadomością nie może być puste");
            }
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void textBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            elementsResize();
            
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            elementsResize();
        }

        private void elementsResize()
        {
            var workingWidth = this.ActualWidth;
            MessageText.Height = ActualHeight * 0.12;
            SearchButton.FontSize = ActualHeight * 0.03;
            ReturnButton.FontSize = ActualHeight * 0.03;
            UserTypeCombo.FontSize = ActualHeight * 0.03;
            NumberTextBox.FontSize = ActualHeight * 0.05;
            NumberTextBox.Height = ActualHeight * 0.07;
            Message.FontSize = ActualHeight * 0.03;
            SendButton.FontSize = ActualHeight * 0.03;
        }
    }
}
