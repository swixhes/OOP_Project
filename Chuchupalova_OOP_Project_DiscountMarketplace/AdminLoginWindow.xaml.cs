using DiscountMarketplace.Domain;
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

namespace Chuchupalova_OOP_Project_DiscountMarketplace
{
    /// <summary>
    /// Логика взаимодействия для AdminLoginWindow.xaml
    /// </summary>
    public partial class AdminLoginWindow : Window
    {
        private const string AdminUsername = "admin@g.com";
        private const string AdminPassword = "admin123!";

        public AdminLoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (username == AdminUsername && password == AdminPassword)
            {
                var admin = new Admin(99, "admin@example.com", "Адмін", "Системи", "+38(000)-0000000", "admin123!");

                var editor = new ACouponEditorWindow(admin);
                editor.ShowDialog();

                this.Close();
            }
            else
            {
                MessageBox.Show("Невірний логін або пароль.");
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var loginPage = new LoginPage();
            this.Close();
            loginPage.ShowDialog();
        }
    }
}
