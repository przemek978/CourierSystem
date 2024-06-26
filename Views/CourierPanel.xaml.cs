﻿using CourierSystem.Data;
using CourierSystem.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.IO;
using TallComponents.PDF;
using TallComponents.PDF.Configuration;
using TallComponents.PDF.Diagnostics;
using TallComponents.PDF.Rasterizer;
using Page = TallComponents.PDF.Page;
using PageSize = iTextSharp.text.PageSize;
using Size = System.Windows.Size;

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
        private List<Shipment> Couriershipments;
        private List<ShipmentStatus> statuses;
        private string selectedShipmentNumber = "";
        private Shipment shipmentToChange;
        private Models.User user;
        public CourierPanel(Models.User user)
        {
            InitializeComponent();
            this.user = user;
            RefreshShipmentListView("");
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource =
                new BitmapImage(new Uri("../../../Background.jpg", UriKind.Relative));
            this.Background = myBrush;
        }

        private void RefreshShipmentListView(String contains1)
        {
            shipmentsViews = new List<ShipmentCourierView>();
            Couriershipments = new List<Shipment>();
            shipments = DB.GetShipmentsWithOtherTables();
            statuses = DB.GetStatuses();
            String contains = contains1.ToLower();
            foreach (var ship in shipments)
            {
                if (ship.CourierID == user.Id && (ship.ShipmentNumber.ToString().Contains(contains)
                    || ship.Status.Status.ToLower().Contains(contains) || 
                    (ship.Sender.FirstName + " " + ship.Sender.LastName + ", " + ship.Sender.Address + ", " + ship.Sender.PhoneNumber).ToLower().Contains(contains)
                    || (ship.Recipient.FirstName + " " + ship.Recipient.LastName + ", " + ship.Recipient.Address + ", " + ship.Recipient.PhoneNumber).ToLower().Contains(contains)
                    || ship.Size.ToString().ToLower().Contains(contains)))
                {
                    if (ship.Status.Id != 8)
                        Couriershipments.Add(ship);
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

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Login window = new Login();
            window.Show();
            this.Close();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            var pdfFilePath = Printer.GenerujListPrzewozowy(Couriershipments, user, true);
            //show a printdialog to user where attributes can be changed
            PrintDialog printDialog = new PrintDialog();
            printDialog.PageRangeSelection = PageRangeSelection.AllPages;
            printDialog.UserPageRangeEnabled = true;
            bool? doPrint = printDialog.ShowDialog();
            if (doPrint != true)
            {
                return;
            }

            //open the pdf file
            FixedDocument fixedDocument;
            using (FileStream pdfFile = new FileStream(pdfFilePath, FileMode.Open, FileAccess.Read))
            {
                Document document = new Document(pdfFile);
                RenderSettings renderSettings = new RenderSettings();
                ConvertToWpfOptions renderOptions = new ConvertToWpfOptions { ConvertToImages = false };
                renderSettings.RenderPurpose = RenderPurpose.Print;

                Summary summary = new Summary();
                fixedDocument = document.ConvertToWpf(renderSettings, renderOptions, 0, 1, summary);
            }
            printDialog.PrintDocument(fixedDocument.DocumentPaginator, "Print");
        }

        private void PrintLabelButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedShipmentNumber.IsNullOrEmpty())
            {
                MessageBox.Show("Najpierw wybierz przesłke dla które chcesz wygenrować etykiete");
            }
            else
            {
                Printer.GenerujEtykiete(selectedShipmentNumber);
            }
        }
        private void ShipmentListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth; // take into account vertical scrollbar
            var col1 = 0.10;
            var col2 = 0.35;
            var col3 = 0.35;
            var col4 = 0.15;
            var col5 = 0.05;

            gView.Columns[0].Width = workingWidth * col1;
            gView.Columns[1].Width = workingWidth * col2;
            gView.Columns[2].Width = workingWidth * col3;
            gView.Columns[3].Width = workingWidth * col4;
            gView.Columns[4].Width = workingWidth * col5;
        }
    }

}
