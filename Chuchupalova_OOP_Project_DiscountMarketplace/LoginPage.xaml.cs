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
                MessageBox.Show($"Вітаємо, {user.FirstName}!", "Успішний вхід", MessageBoxButton.OK, MessageBoxImage.Information);  
                var userWindow = new RegisteredUserWindow(user);
                this.Close();
                userWindow.Show();
                
            }
            else
            {
                MessageBox.Show("Невірний email або пароль.", "Помилка входу", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
        }
    }
}
