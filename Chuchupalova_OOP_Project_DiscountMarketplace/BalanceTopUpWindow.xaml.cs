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
using System.Windows.Threading;

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
            if (!double.TryParse(AmountBox.Text, out double amount))
            {
                MessageBox.Show("Введіть коректну числову суму.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                AmountBox.Clear();
                return;
            }

            if (!user.TopUpBalance(amount))
            {
                string msg = amount <= 0
                    ? "Сума має бути більшою за 0."
                    : "Сума поповнення не може перевищувати 5000 грн.";
                MessageBox.Show(msg, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                AmountBox.Clear();
                return;
            }

            var method = (PaymentMethodBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Банківська карта";

            JsonStorage.SaveUsersToJson(RegisteredUser.GetAllUsers());

            SuccessToast.Text = $"Рахунок поповнено на {amount:0.00} грн через {method}.";
            SuccessBorder.Visibility = Visibility.Visible;

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            timer.Tick += (s, args) =>
            {
                SuccessBorder.Visibility = Visibility.Collapsed;
                timer.Stop();
            };
            timer.Start();

            CurrentBalanceText.Text = $"{user.Balance:0.00} грн";
            AmountBox.Clear();
        }



        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
