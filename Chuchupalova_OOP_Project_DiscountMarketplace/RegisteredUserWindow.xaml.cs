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
    /// Логика взаимодействия для RegisteredUserWindow.xaml
    /// </summary>
    public partial class RegisteredUserWindow : Window
    {
        
        private RegisteredUser user;
        public RegisteredUserWindow(RegisteredUser loggedInUser)
        {
            InitializeComponent();
            user = loggedInUser;
            DataContext = user;
            Loaded += RegisteredUserWindow_Loaded;
        }

        private void RegisteredUserWindow_Loaded(object sender, RoutedEventArgs e)
        {
            JsonStorage.LoadCouponsFromJson();
            JsonStorage.LoadReviewsFromJson(RegisteredUser.GetAllUsers());
            JsonStorage.LoadOrdersFromJson(RegisteredUser.GetAllUsers(), Coupon.GetAllCoupons());
       
            //Coupon.InitializeTestCoupons();
            LoadCoupons();
         
            //JsonStorage.LoadOrdersFromJson(RegisteredUser.GetAllUsers(), Coupon.GetAllCoupons());

        }

        private void LoadCoupons(CouponCategory? category = null)
            {

                if (CouponList == null) return;

                string searchText = SearchBox?.Text?.ToLower() ?? "";
                CouponList.Items.Clear();

                var coupons = Coupon.GetAllCoupons();
                if (category.HasValue)
                    coupons = coupons.Where(c => c.Category == category.Value).ToList();

                if (!string.IsNullOrWhiteSpace(searchText))
                    coupons = coupons.Where(c => c.Name.ToLower().Contains(searchText)).ToList();

                foreach (var coupon in coupons)
                {
                    var border = new Border
                    {
                        BorderThickness = new Thickness(1),
                        BorderBrush = Brushes.LightGray,
                        Margin = new Thickness(10),
                        CornerRadius = new CornerRadius(10),
                        Background = Brushes.WhiteSmoke,
                        Padding = new Thickness(10)
                    };

                    var panel = new StackPanel { Margin = new Thickness(10) };

                    if (!string.IsNullOrWhiteSpace(coupon.ImagePath) && System.IO.File.Exists(coupon.ImagePath))
                    {
                        var image = new Image
                        {
                            Width = 250,
                            Height = 150,
                            Stretch = Stretch.UniformToFill,
                            Margin = new Thickness(0, 0, 0, 10),
                            Source = new BitmapImage(new Uri(coupon.ImagePath, UriKind.Relative))
                        };
                        panel.Children.Add(image);
                    }

                    panel.Children.Add(new TextBlock { Text = coupon.Name, FontWeight = FontWeights.Bold, FontSize = 16 });
                    panel.Children.Add(new TextBlock { Text = $"Ціна: {coupon.Price} грн", Foreground = Brushes.OrangeRed });
                    panel.Children.Add(new TextBlock { Text = $"Знижка: {coupon.Discount}%" });
                    panel.Children.Add(new TextBlock { Text = $"Дійсний до: {coupon.ExpirationDate:dd.MM.yyyy}" });
                    panel.Children.Add(new TextBlock { Text = $"Категорія: {coupon.Category}" });
                    panel.Children.Add(new TextBlock { Text = $"Середній рейтинг: {Review.GetAverageRating(coupon.Id):0.0}" });

                    var btn = new Button
                    {
                        Content = "Детальніше",
                        Tag = coupon,
                        Margin = new Thickness(0, 5, 0, 0),
                        Background = Brushes.DarkOrange,
                        BorderBrush = Brushes.DarkOrange,
                        Foreground = Brushes.White
                    };
                    btn.Click += ViewDetails_Click;
                    panel.Children.Add(btn);

                    border.Child = panel;
                    CouponList.Items.Add(new ListBoxItem { Content = border });
                }
            }

            private void CategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                var selected = (CategoryFilter.SelectedItem as ComboBoxItem)?.Content?.ToString();
                if (selected == "Усі категорії")
                    LoadCoupons();
                else if (Enum.TryParse(selected, out CouponCategory selectedCategory))
                    LoadCoupons(selectedCategory);
            }

            private void ViewDetails_Click(object sender, RoutedEventArgs e)
            {
                var coupon = (sender as Button)?.Tag as Coupon;
                if (coupon != null)
                {
                    var couponDetailsForRUserWindow = new CouponDetailsForRUserWindow(coupon,user);
                    this.Close();
                couponDetailsForRUserWindow.ShowDialog();
                }
            }

            private void SearchBox_TextChanged(object sender, KeyEventArgs e)
            {
                string input = SearchBox.Text.ToLower();

                if (string.IsNullOrWhiteSpace(input))
                {
                    SearchBox.ItemsSource = null;
                    SearchBox.IsDropDownOpen = false;
                    LoadCoupons();
                    return;
                }

                var suggestions = Coupon.GetAllCoupons()
                    .Where(c => c.Name.ToLower().Contains(input))
                    .Select(c => c.Name)
                    .Distinct()
                    .ToList();

                if (SearchBox.ItemsSource == null || !((IEnumerable<string>)SearchBox.ItemsSource).SequenceEqual(suggestions))
                    SearchBox.ItemsSource = suggestions;

                SearchBox.IsDropDownOpen = suggestions.Any();
                LoadCoupons();
            }

            private void SearchButton_Click(object sender, RoutedEventArgs e)
            {
                LoadCoupons();
            }

            private void CartButton_Click(object sender, RoutedEventArgs e)
            {
            var cartWindow = new CartWindow(user);
            cartWindow.Show();
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            var profileWindow = new UserProfileWindow(user);
            profileWindow.ShowDialog();
        }

        private void MyCoupons_Click(object sender, RoutedEventArgs e)
            {
            var myCouponsWindow = new MyCouponsWindow(user);
            myCouponsWindow.ShowDialog();
            
            }

            private void TopUp_Click(object sender, RoutedEventArgs e)
            {
            var window = new BalanceTopUpWindow(user); // currentUser — авторизований користувач
            window.ShowDialog();
        }

            private void Logout_Click(object sender, RoutedEventArgs e)
            {
                new MainWindow().Show();
                this.Close();
            }
        
    }
}
