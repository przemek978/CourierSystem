using CourierSystem.Views;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CourierSystem.Data;

namespace CourierSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource =
                new BitmapImage(new Uri("../../../Background.jpg", UriKind.Relative));
            this.Background = myBrush;
            //Window window = new CourierPanel(DB.GetCouriers()[0]);
            //Window window = new ShipIndex(DB.GetInstance().Users.ToList()[3]);
            //window.Show();
            //this.Close();
        }

        private void Tracking_Click(object sender, RoutedEventArgs e)
        {
            ShipTracking shipTracking = new ShipTracking();
            shipTracking.Show();
            this.Close();
        }

        private void Navigate_To_Login(object sender, RoutedEventArgs e)
        {
            Login loginView = new Login();
            loginView.Show();
            this.Close();
        }
    }
}
