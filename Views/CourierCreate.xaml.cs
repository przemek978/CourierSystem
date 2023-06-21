using CourierSystem.Data;
using CourierSystem.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
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
    /// Logika interakcji dla klasy CourierCreate.xaml
    /// </summary>
    public partial class CourierCreate : Window
    {
        private Courier courier { get; set; }
        public CourierCreate()
        {
            InitializeComponent();
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource =
                new BitmapImage(new Uri("../../../Background.jpg", UriKind.Relative));
            this.Background = myBrush;
            
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                courier = new Courier (CourierName.Text, CourierUsername.Text, "Kurier", Password.Password);
                courier.PasswordHash();
                DB.AddCourier(courier);
                MessageBox.Show("Kurier dodany pomyślnie");
                CourierManagement courierManagement = new CourierManagement();
                courierManagement.Show();
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CourierManagement window = new CourierManagement();
            window.Show();
            this.Close();
        }

        private bool CheckValidation()
        {
            string message = "";
            bool IsValid = true;
            if (CourierUsername.Text.Length<5)
            {
                message += "Nazwa użytkownika jest za krótka(min. 4 litery)\n";
            }
            if (!DB.CheckUsername(CourierUsername.Text))
            {
                message += "Nazwa użytkownika "+ CourierUsername.Text + " jest już zajęta\n";
                IsValid = false;
            }
            if (CourierName.Text.IsNullOrEmpty())
            {
                message += "Pole imię nie może być puste\n";
                IsValid = false;
            }
            if (Password.Password !=ConfirmPassword.Password)
            {
                message += "Hasła muszą być takie same\n";
                IsValid = false;
            }
            if (Password.Password.IsNullOrEmpty() || ConfirmPassword.Password.IsNullOrEmpty())
            {
                message += "Pole hasło jest wymagane\n";
                IsValid = false;
            }
            if (!IsValid) MessageBox.Show(message);
            return IsValid;
        }

    }
}
