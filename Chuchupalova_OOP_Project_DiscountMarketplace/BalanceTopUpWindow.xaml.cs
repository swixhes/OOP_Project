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
    /// Логика взаимодействия для BalanceTopUpWindow.xaml
    /// </summary>
    public partial class BalanceTopUpWindow : Window
    {
        private RegisteredUser user;

        public BalanceTopUpWindow(RegisteredUser user)
        {
            InitializeComponent();
            this.user = user;
            CurrentBalanceText.Text = $"{user.Balance:0.00} грн";
            PaymentMethodBox.SelectedIndex = 0;
        }

        private void TopUp_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(AmountBox.Text, out double amount) || amount <= 0)
            {
                MessageBox.Show("Введіть коректну суму для поповнення.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var method = (PaymentMethodBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Банківська карта";

            user.Balance += amount;
            JsonStorage.SaveUsersToJson(RegisteredUser.GetAllUsers());

            MessageBox.Show($"Рахунок поповнено на {amount:0.00} грн через {method}.", "Успішно", MessageBoxButton.OK, MessageBoxImage.Information);
            CurrentBalanceText.Text = $"{user.Balance:0.00} грн";

            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
