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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chuchupalova_OOP_Project_DiscountMarketplace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Guest guest;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            JsonStorage.LoadUsersFromJson();
            JsonStorage.LoadCouponsFromJson();

            JsonStorage.LoadReviewsFromJson(RegisteredUser.GetAllUsers());
            JsonStorage.LoadOrdersFromJson(RegisteredUser.GetAllUsers(), Coupon.GetAllCoupons());

            guest = new Guest(1, "guest@dealhive.com", "Гість", "Системи", "+38(000)-0000000");
            foreach (CouponCategory cat in Enum.GetValues(typeof(CouponCategory)))
                guest.AvailableCategories.Add(cat);

            Coupon.InitializeTestCoupons();
            LoadCoupons();
        }

        private void LoadCoupons(CouponCategory? category = null)
        {
            if (CouponList == null) return;

            string searchText = SearchBox?.Text?.ToLower() ?? "";
            CouponList.Items.Clear();

            var coupons = guest.ViewCoupons();
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

                // Зображення
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
                    BorderThickness = new Thickness(0),
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
                
                var detailsWindow = new CouponDetailsWindow(coupon);
                this.Close();
                detailsWindow.ShowDialog();

            }
        }


        private void SearchBox_TextChanged(object sender, KeyEventArgs e)
        {
            string input = SearchBox.Text.ToLower();

            if (string.IsNullOrWhiteSpace(input))
            {
                SearchBox.ItemsSource = null;
                SearchBox.IsDropDownOpen = false;
                LoadCoupons(); // показати всі купони
                return;
            }

            var suggestions = guest.ViewCoupons()
                                   .Where(c => c.Name.ToLower().Contains(input))
                                   .Select(c => c.Name)
                                   .Distinct()
                                   .ToList();

            // оновлюємо лише якщо список змінився
            if (SearchBox.ItemsSource == null || !((IEnumerable<string>)SearchBox.ItemsSource).SequenceEqual(suggestions))
            {
                SearchBox.ItemsSource = suggestions;
            }

            SearchBox.IsDropDownOpen = suggestions.Any();

            LoadCoupons();
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginPage();
            loginWindow.Show();
            this.Close();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            LoadCoupons();
        }
        private void OpenTopBuyers_Click(object sender, RoutedEventArgs e)
        {
            var topBuyersWindow = new TopBuyersWindow();
            topBuyersWindow.ShowDialog();
        }
        private void HelpText_Click(object sender, MouseButtonEventArgs e)
        {
            var helpWindow = new HelpWindow();
            helpWindow.ShowDialog();
        }

    }
}
