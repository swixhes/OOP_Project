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
using DiscountMarketplace.Domain;


namespace Chuchupalova_OOP_Project_DiscountMarketplace
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
            JsonStorage.LoadUsersFromJson(); // завантаження при старті
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;

            var user = RegisteredUser.GetAllUsers()
                .FirstOrDefault(u => u.Email == email);

            if (user != null && user.Login(email, password))
            {
                //MessageBox.Show($"Вітаємо, {user.FirstName}!", "Успішний вхід", MessageBoxButton.OK, MessageBoxImage.Information);  
                var userWindow = new RegisteredUserWindow(user);
                this.Close();
                userWindow.Show();
                
            }
            else
            {
                    ErrorTextBlock.Visibility = Visibility.Visible;
                
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new RegistrationWindow();
            this.Close();
            registrationWindow.Show();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == EmailTextBox)
            {
                EmailPlaceholder.Visibility = string.IsNullOrEmpty(EmailTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
                ErrorTextBlock.Visibility = Visibility.Collapsed;
            }
            
        }

        private bool isPasswordVisible = false;

        private void TogglePasswordVisibility(object sender, RoutedEventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                VisiblePasswordBox.Text = PasswordBox.Password;
                PasswordBox.Visibility = Visibility.Collapsed;
                VisiblePasswordBox.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordBox.Password = VisiblePasswordBox.Text;
                PasswordBox.Visibility = Visibility.Visible;
                VisiblePasswordBox.Visibility = Visibility.Collapsed;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!isPasswordVisible)
                VisiblePasswordBox.Text = PasswordBox.Password;
        }

        private void VisiblePasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isPasswordVisible)
                PasswordBox.Password = VisiblePasswordBox.Text;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }
        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new AdminLoginWindow();
            this.Close();
            loginWindow.ShowDialog();
        }

    }
}
