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
    /// Логика взаимодействия для CouponDetailsWindow.xaml
    /// </summary>
    public partial class CouponDetailsWindow : Window
    {
        public CouponDetailsWindow(Coupon coupon)
        {
            InitializeComponent();

            if (coupon == null)
            {
                MessageBox.Show("Купон не знайдено", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            CouponName.Text = coupon.Name;
            CouponCategory.Text = $"Категорія: {coupon.Category}";
            CouponPrice.Text = $"{coupon.Price} грн";
            CouponDiscount.Text = $"Знижка: {coupon.Discount}%";
            CouponExpiration.Text = $"Дійсний до: {coupon.ExpirationDate:dd.MM.yyyy}";
            CouponRating.Text = $"Середній рейтинг: {Review.GetAverageRating(coupon.Id):0.0}";
            CouponPurchaseCount.Text = $"Придбано: {coupon.PurchaseCount} раз(ів)";
            CouponUsageLimit.Text = $"Ліміт використань: {coupon.UsageLimit} раз(ів)";


            // Опис (можеш змінити під себе)
            CouponDescription.Text = coupon.Description;


            // Фото
            if (!string.IsNullOrWhiteSpace(coupon.ImagePath) && System.IO.File.Exists(coupon.ImagePath))
            {
                CouponImage.Source = new BitmapImage(new Uri(coupon.ImagePath, UriKind.Relative));
            }

            // Відгуки
            ReviewList.ItemsSource = null;
            ReviewList.ItemsSource = Review.GetReviewsByCouponId(coupon.Id);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            
            var mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }
    }
}
