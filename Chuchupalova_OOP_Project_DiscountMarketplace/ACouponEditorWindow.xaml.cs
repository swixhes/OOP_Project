using DiscountMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Логика взаимодействия для ACouponEditorWindow.xaml
    /// </summary>
    public partial class ACouponEditorWindow : Window
    {
        private Coupon editingCoupon;
        private readonly ICouponManagement couponManager;

        public ACouponEditorWindow(ICouponManagement couponManager, Coupon coupon = null)
        {
            InitializeComponent();
            this.couponManager = couponManager ?? throw new ArgumentNullException(nameof(couponManager));

            CategoryComboBox.ItemsSource = Enum.GetValues(typeof(CouponCategory));

            if (coupon != null)
            {
                editingCoupon = coupon;
                IdTextBox.Text = coupon.Id.ToString();
                NameTextBox.Text = coupon.Name;
                CategoryComboBox.SelectedItem = coupon.Category;
                DiscountTextBox.Text = coupon.Discount.ToString();
                ExpirationDatePicker.SelectedDate = coupon.ExpirationDate;
                UsageLimitTextBox.Text = coupon.UsageLimit.ToString();
                DescriptionTextBox.Text = coupon.Description;
            }
            else
            {
                int newId = Coupon.GetAllCoupons().Any() ? Coupon.GetAllCoupons().Max(c => c.Id) + 1 : 1;
                IdTextBox.Text = newId.ToString();
                ExpirationDatePicker.SelectedDate = DateTime.Now.AddDays(30);
                CategoryComboBox.SelectedIndex = 0;
                IdTextBox.IsReadOnly = false;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = int.Parse(IdTextBox.Text);
                string name = NameTextBox.Text;
                CouponCategory category = (CouponCategory)CategoryComboBox.SelectedItem;
                double discount = double.Parse(DiscountTextBox.Text);
                DateTime expirationDate = ExpirationDatePicker.SelectedDate ?? DateTime.Now;
                int usageLimit = int.Parse(UsageLimitTextBox.Text);
                string description = DescriptionTextBox.Text;

                var coupon = new Coupon(id, name, category, expirationDate, discount, usageLimit, "Images/default.jpg", description);

                if (!couponManager.CreateOrEditCoupon(coupon))
                {
                    MessageBox.Show("Не вдалося зберегти купон", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при збереженні купона: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
