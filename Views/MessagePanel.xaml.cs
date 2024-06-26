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
    /// Logika interakcji dla klasy MessagePanel.xaml
    /// </summary>
    public partial class MessagePanel : Window
    {
        private class MessageView
        {
            public string UserType { get; set; }
            public string Content { get; set; }
            public long ShipmentNumber { get; set; }
            public string Status { get; set; }
        }

        private List<MessageView> messagesViews;
        private List<Message> messages;
        private int selectedMessageInd = -1;
        private string selectedMessage = "";
        private Message messageToChange;
        private bool CheckClosed = true;
        public MessagePanel()
        {
            InitializeComponent();
            RefreshMessageListView();
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource =
                new BitmapImage(new Uri("../../../Background.jpg", UriKind.Relative));
            this.Background = myBrush;
        }

        public void RefreshMessageListView(String text1 = "")
        {
            messagesViews = new List<MessageView>();
            messages = DB.GetMessages();
            String text = text1.ToLower();
            foreach (var mess in messages)
            {
                if (CheckClosed == true || mess.Status == false)
                {
                    if (mess.UserType.Contains(text) || mess.Content.Contains(text) || mess.ShipmentNumber.ToString().Contains(text))
                    {
                        messagesViews.Add(
                            new MessageView
                            {
                                UserType = mess.UserType,
                                Content = mess.Content,
                                ShipmentNumber = mess.ShipmentNumber,
                                Status = mess.Status == true ? "Tak" : "Nie"
                            });
                    }
                }
            }
            ListViewMessages.ItemsSource = messagesViews;
            ListViewMessages.Items.Refresh();
            DataContext = this;

        }

        private void Closed_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded)
                return;
            CheckClosed = true;
            RefreshMessageListView();
        }

        private void Closed_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckClosed = false;
            RefreshMessageListView();
        }

        private void ListViewMessage_ItemClicked(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = (ListView)sender;
            if (listView.SelectedItem != null)
            {
                selectedMessageInd = listView.SelectedIndex;
                selectedMessage = ((MessageView)listView.SelectedItem).Content;
            }
        }

        private void StatusButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedMessageInd == -1)
            {
                MessageBox.Show("Najpierw wybierz element, ktoremu chcesz zmienić status");
            }
            else
            {
                messageToChange = DB.SearchMessage(selectedMessage);
                if (messageToChange != null)
                {
                    messageToChange.Status = !messageToChange.Status;
                    DB.ChangeMessageStatus(messageToChange);
                    RefreshMessageListView();
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

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            String text = SearchMessage.Text;
            RefreshMessageListView(text);
        }

        private void MessageListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth; // take into account vertical scrollbar
            var col1 = 0.15;
            var col2 = 0.50;
            var col3 = 0.15;
            var col4 = 0.20;

            gView.Columns[0].Width = workingWidth * col1;
            gView.Columns[1].Width = workingWidth * col2;
            gView.Columns[2].Width = workingWidth * col3;
            gView.Columns[3].Width = workingWidth * col4;
        }

    }
}
