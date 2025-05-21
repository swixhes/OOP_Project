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
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private Admin currentAdmin;
        private List<Review> allReviews;
        private List<Coupon> allCoupons;

        public AdminWindow(Admin admin)
        {
            InitializeComponent();
            currentAdmin = admin;
            LoadData();
        }

        private void LoadData()
        {
            // Завантаження відгуків
            allReviews = Review.GetAllReviews();
            ReviewsDataGrid.ItemsSource = allReviews;


            // Завантаження купонів
            JsonStorage.LoadCouponsFromJson();
            allCoupons = Coupon.GetAllCoupons();
            CouponsDataGrid.ItemsSource = allCoupons;
        }

        private void DeleteReview_Click(object sender, RoutedEventArgs e)
        {
            if (ReviewsDataGrid.SelectedItem is Review selectedReview)
            {
                if (currentAdmin.DeleteReview(selectedReview.Id))
                {
                    MessageBox.Show("Відгук видалено.");
                    LoadData();
                    JsonStorage.SaveReviewsToJson(Review.GetAllReviews());
                }
                else
                {
                    MessageBox.Show("Не вдалося видалити відгук.");
                }
            }
            else
            {
                MessageBox.Show("Оберіть відгук для видалення.");
            }
        }

        private void EditCoupon_Click(object sender, RoutedEventArgs e)
        {

            if (CouponsDataGrid.SelectedItem is Coupon selectedCoupon)
            {
                var couponWindow = new ACouponEditorWindow(currentAdmin, selectedCoupon);
                if (couponWindow.ShowDialog() == true)
                {
                    LoadData();
                    JsonStorage.SaveCouponsToJson(Coupon.GetAllCoupons());
                }
            }
            else
            {
                MessageBox.Show("Оберіть купон для редагування.");
            }
        }

        private void ExitCoupon_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
            
        }
    }
}
