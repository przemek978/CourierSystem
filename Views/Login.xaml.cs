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
    /// Logika interakcji dla klasy Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private User user;
        public Login()
        {
            InitializeComponent();
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource =
                new BitmapImage(new Uri("D:/Projects/CourierSystem/Background.jpg", UriKind.Absolute));
            this.Background = myBrush;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var user=DB.ValidateUser(Username.Text, password.Password);
            if (user == null)
            {
                MessageBox.Show("Błędny email lub haslo");
            }
            else
            {
                this.user = user;
                OpenNextView();
            }
        }

        public void OpenNextView()
        {
            Window win;
            if (user.Role=="Admin")
            {
                win = new AdminPanel();
            }
            else
            {
                win = new CourierPanel(user);
            }
            win.Show();
            this.Close();
        }
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

    }
}
