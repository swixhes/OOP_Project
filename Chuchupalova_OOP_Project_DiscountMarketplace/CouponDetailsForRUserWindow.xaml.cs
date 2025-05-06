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
    /// Логика взаимодействия для CouponDetailsForRUserWindow.xaml
    /// </summary>
    public partial class CouponDetailsForRUserWindow : Window
    {
        private readonly Coupon coupon;
        private readonly RegisteredUser user;

        public CouponDetailsForRUserWindow(Coupon coupon, RegisteredUser user)
        { 
            InitializeComponent();
            JsonStorage.LoadCouponsFromJson();
            

            this.coupon = coupon ?? throw new ArgumentNullException(nameof(coupon));
            this.user = user ?? throw new ArgumentNullException(nameof(user));

            LoadCouponDetails();

            LoadReviews();
        }

        private void LoadCouponDetails()
        {
            CouponName.Text = coupon.Name;
            CouponCategory.Text = $"Категорія: {coupon.Category}";
            CouponPrice.Text = $"{coupon.Price} грн";
            CouponDiscount.Text = $"Знижка: {coupon.Discount}%";
            CouponExpiration.Text = $"Дійсний до: {coupon.ExpirationDate:dd.MM.yyyy}";
            CouponRating.Text = $"Середній рейтинг: {Review.GetAverageRating(coupon.Id):0.0}";
            CouponPurchaseCount.Text = $"Придбано: {coupon.PurchaseCount} раз(ів)";
            CouponUsageLimit.Text = $"Ліміт використань: {coupon.UsageLimit} раз(ів)";
            CouponDescription.Text = coupon.Description;

            if (!string.IsNullOrWhiteSpace(coupon.ImagePath) && System.IO.File.Exists(coupon.ImagePath))
            {
                CouponImage.Source = new BitmapImage(new Uri(coupon.ImagePath, UriKind.Relative));
            }
        }

        private void LoadReviews()
        {

            ReviewList.ItemsSource = null;
            ReviewList.ItemsSource = Review.GetReviewsByCouponId(coupon.Id)
    .Select(r => new
    {
        Review = r,
        IsOwner = r.Author.Id == user.Id
    }).ToList();

            //JsonStorage.LoadReviewsFromJson(RegisteredUser.GetAllUsers());
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new RegisteredUserWindow(user).Show();
            this.Close();
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Order.AddToCart(user, coupon);
                MessageBox.Show("Купон додано до кошика.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сталася непередбачена помилка: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            var cartWindow = new CartWindow(user);
            cartWindow.Show();
            //Order.AddToCart(user, coupon);
            //MessageBox.Show("Купон додано до кошика.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SubmitReview_Click(object sender, RoutedEventArgs e)
        {
            if (!user.PurchasedCoupons.Any(o => o.Coupon.Id == coupon.Id))
            {
                MessageBox.Show("Ви можете залишати відгуки лише на придбані купони.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string comment = ReviewBox.Text.Trim();
            if (!(RatingBox.SelectedItem is ComboBoxItem selectedItem && int.TryParse(selectedItem.Content.ToString(), out int rating)))
            {
                MessageBox.Show("Оберіть оцінку перед відправкою відгуку.", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                MessageBox.Show("Відгук не може бути порожнім.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            new Review(coupon.Id, user, rating, comment);
            MessageBox.Show("Відгук надіслано.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            ReviewBox.Clear();
            RatingBox.SelectedIndex = -1;
            LoadReviews();
            JsonStorage.SaveReviewsToJson(Review.GetAllReviews());

        }
        private void DeleteReview_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dataContext = button?.DataContext;

            // Витягуємо сам Review з анонімного об’єкта
            var reviewProperty = dataContext?.GetType().GetProperty("Review");
            var review = reviewProperty?.GetValue(dataContext) as Review;
            if (review == null) return;

            if (user.DeleteReview(review.Id))
            {
                MessageBox.Show("Відгук видалено.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                JsonStorage.SaveReviewsToJson(Review.GetAllReviews());
                LoadReviews();
            }
            else
            {
                MessageBox.Show("Не вдалося видалити відгук.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
