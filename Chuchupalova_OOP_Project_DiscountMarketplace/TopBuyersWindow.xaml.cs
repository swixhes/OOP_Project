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
    /// Логика взаимодействия для TopBuyersWindow.xaml
    /// </summary>
    public partial class TopBuyersWindow : Window
    {
        public TopBuyersWindow()
        {
            InitializeComponent();
            LoadTopBuyers();
        }

        private void LoadTopBuyers()
        {
            JsonStorage.LoadUsersFromJson(); // Завантаження при відкритті
            var sortedUsers = RegisteredUser.GetAllUsers()
                                            .Where(u => u.PurchasedCoupons.Count > 0)
                                            .OrderByDescending(u => u.PurchasedCoupons.Count)
                                            .ToList();

            BuyersDataGrid.ItemsSource = sortedUsers;
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
