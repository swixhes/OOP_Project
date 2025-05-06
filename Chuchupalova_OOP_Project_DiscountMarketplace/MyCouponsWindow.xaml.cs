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
    /// Логика взаимодействия для MyCouponsWindow.xaml
    /// </summary>
    public partial class MyCouponsWindow : Window
    {
        private readonly RegisteredUser currentUser;

        public class CouponViewModel
        {
            public int CouponId { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }
            public DateTime PurchaseDate { get; set; }
            public string Status { get; set; }
            public bool IsReturnable { get; set; }
        }

        public MyCouponsWindow(RegisteredUser user)
        {
            InitializeComponent();
            
            currentUser = user ?? throw new ArgumentNullException(nameof(user));
            LoadCoupons();
        }

        private void LoadCoupons()
        {

            var coupons = currentUser.PurchasedCoupons
                .Select(o => new CouponViewModel
                {
                    CouponId = o.Coupon.Id,
                    Name = o.Coupon.Name,
                    Price = o.Coupon.Price,
                    PurchaseDate = o.PurchaseDate,
                    Status = o.Coupon.IsValid() ? "Доступний" : "Сплив термін",
                    IsReturnable = o.Coupon.IsValid() && (DateTime.Now - o.PurchaseDate).TotalHours <= 1
                }).ToList();

            CouponsDataGrid.ItemsSource = coupons;
            NoCouponsMessage.Visibility = coupons.Any() ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ReturnCoupon_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Tag.ToString(), out int couponId))
            {
                bool success = currentUser.ReturnCoupon(couponId);
                if (success)
                {
                    JsonStorage.SaveUsersToJson(RegisteredUser.GetAllUsers());
                    JsonStorage.SaveOrdersToJson(RegisteredUser.GetAllUsers().SelectMany(u => u.ViewPurchasedCoupons()).ToList());
                    MessageBox.Show("Купон повернено успішно.", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadCoupons();
                }
                else
                {
                    MessageBox.Show("Не вдалося повернути купон.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
