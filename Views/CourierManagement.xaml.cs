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
    /// Logika interakcji dla klasy CourierManagement.xaml
    /// </summary>
    public partial class CourierManagement : Window
    {
        private class CourierView
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Username { get; set; }
            public string Role { get; set; }
            public int ShipmentsCount { get; set; }
        }

        private List<CourierView> couriersViews;
        private List<Courier> couriers;
        private int selectedCourierId = -1;
        private Courier courierToChange;

        public CourierManagement()
        {
            InitializeComponent();
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource =
                new BitmapImage(new Uri("../../../Background.jpg", UriKind.Relative));
            this.Background = myBrush;
            RefreshCourierListView("");
        }

        public void RefreshCourierListView(String content1)
        {
            couriersViews = new List<CourierView>();
            couriers = DB.GetCouriers();
            int count = 0;
            String content = content1.ToLower();
            foreach (var courier in couriers)
            {
                count = DB.CountCourierShipments(courier.Id);
                if (courier.Id.ToString().ToLower().Contains(content) || courier.Name.ToLower().Contains(content) || courier.Username.ToLower().Contains(content)
                    || courier.Role.ToLower().Contains(content) || count.ToString().Contains(content))
                {
                    couriersViews.Add(
                        new CourierView
                        {
                            Id = courier.Id,
                            Name = courier.Name,
                            Username = courier.Username,
                            Role = courier.Role,
                            ShipmentsCount = count
                        });
                }
            }
            ListViewCourier.ItemsSource = couriersViews;
            ListViewCourier.Items.Refresh();
            DataContext = this;
        }

        private void ListViewCourier_ItemClicked(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = (ListView)sender;
            if (listView.SelectedItem != null)
            {
                CourierView selectedCourierView = (CourierView)listView.SelectedItem;
                selectedCourierId = selectedCourierView.Id;
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CourierCreate createCourier = new CourierCreate();
            createCourier.Show();
            this.Close();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCourierId==-1)
            {
                MessageBox.Show("Najpierw wybierz wiersz, który chcesz edytować");
            }
            else
            {
                courierToChange = DB.SearchCourier(selectedCourierId);
                if (courierToChange != null)
                {
                    CourierEdit courierEdit = new CourierEdit(courierToChange);//, this);
                    courierEdit.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Coś poszło nie tak, operacja anulowana.");
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCourierId == -1)
            {
                MessageBox.Show("Najpierw wybierz, który chcesz usunąć");
            }
            else
            {
                
                courierToChange = DB.SearchCourier(selectedCourierId);
                if (courierToChange != null)
                {
                    int ccs = DB.CountCourierShipments(selectedCourierId);
                    if (ccs > 0)
                    {
                        MessageBox.Show("Nie można usunąć kuriera, który ma przesyłki do dostarczenia (Przydziel je innemu kurierowi).");
                    }
                    else
                    {
                        if (MessageBox.Show("Czy na pewno chcesz usunąć kuriera z id: " + selectedCourierId + "?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            DB.DeleteCourier(courierToChange);
                            MessageBox.Show("Usunięto pomyślnie");
                            RefreshCourierListView("");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Coś poszło nie tak, operacja anulowana.");
                }

            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel window = new AdminPanel();
            window.Show();
            this.Close();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            String text = SearchCourier.Text;
            RefreshCourierListView(text);
        }

        private void CourierListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth; // take into account vertical scrollbar
            var col1 = 0.15;
            var col2 = 0.15;
            var col3 = 0.25;
            var col4 = 0.15;
            var col5 = 0.30;

            gView.Columns[0].Width = workingWidth * col1;
            gView.Columns[1].Width = workingWidth * col2;
            gView.Columns[2].Width = workingWidth * col3;
            gView.Columns[3].Width = workingWidth * col4;
            gView.Columns[4].Width = workingWidth * col5;
        }

    }
}
