using CourierSystem.Data;
using CourierSystem.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
    /// Logika interakcji dla klasy CourierEdit.xaml
    /// </summary>
    public partial class CourierEdit : Window
    {
        Courier courierToChange;

        public CourierEdit(Courier courier)
        {
            InitializeComponent();
            courierToChange = courier; 
            CourierNameLabel.Content = "Zmień imię z " + courierToChange.Name + " na:";
            CourierUsernameLabel.Content = "Zmień nazwę użytkownika z " + courierToChange.Username + " na:";
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource =
                new BitmapImage(new Uri("../../../Background.jpg", UriKind.Relative));
            this.Background = myBrush;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                if (CourierName.Text.Length > 0)
                {
                    courierToChange.Name = CourierName.Text;
                }
                if (CourierUsername.Text.Length > 0)
                {
                    courierToChange.Username = CourierUsername.Text;
                }
                if (!Password.Text.IsNullOrEmpty() && !ConfirmPassword.Text.IsNullOrEmpty())
                {
                    courierToChange.Password = Password.Text;
                    courierToChange.PasswordHash();
                }
                DB.EditCourier(courierToChange);
                MessageBox.Show("Zmiany zapisano pomyślnie!");
                CourierManagement window = new CourierManagement();
                window.Show();
                this.Close();
            }
        }

        private bool CheckValidation()
        {
            string message = "";
            bool IsValid = true;
            if (CourierUsername.Text.Length < 5 && CourierUsername.Text.Length>0)
            {
                message+="Nazwa użytkownika jest za krótka\n";
                IsValid=false;
            }
            else
            {
                if (!DB.CheckUsername(CourierUsername.Text))
                {
                    message+="Nazwa użytkownika " + CourierUsername.Text + " jest już zajęta\n";
                    IsValid=false;
                }
            }
            if (Password.Text != ConfirmPassword.Text)
            {
                message += "Hasła muszą być takie same\n";
                IsValid=false;
            }
            if (Password.Text.Length > 0 && Password.Text.Length < 8)
            {
                message += "Hasło jest za którkie\n";
                IsValid = false;
            }
            if (!IsValid) MessageBox.Show(message);
            return IsValid;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CourierManagement window = new CourierManagement();
            window.Show();
            this.Close();
        }
    }
}
