﻿using DiscountMarketplace.Domain;
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
    /// Логика взаимодействия для CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        private readonly RegisteredUser user;
        private List<CartItemModel> cartItems;

        public CartWindow(RegisteredUser user)
        {
            InitializeComponent();
            this.user = user ?? throw new ArgumentNullException(nameof(user));
            LoadCartItems();
        }

        private void LoadCartItems()
        {
            cartItems = Order.GetCart(user);
            CartList.ItemsSource = null;
            CartList.ItemsSource = cartItems;
            TotalPriceText.Text = $"Разом: {cartItems.Sum(ci => ci.TotalPrice)} грн";
        }

        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is CartItemModel item)
            {
                Order.IncreaseQuantity(user, item.Coupon);
                LoadCartItems();
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is CartItemModel item)
            {
                Order.DecreaseQuantity(user, item.Coupon);
                LoadCartItems();
            }
        }

        private void RemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is CartItemModel item)
            {
                Order.RemoveFromCart(user, item.Coupon);
                LoadCartItems();
            }
        }

        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Order.PurchaseConfirmed += msg =>
                {
                    OrderSuccessToast.Text = msg;
                    OrderSuccessBorder.Visibility = Visibility.Visible;

                    var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
                    timer.Tick += (s, args) =>
                    {
                        OrderSuccessBorder.Visibility = Visibility.Collapsed;
                        timer.Stop();
                    };
                    timer.Start();
                };

                Order.PlaceOrder(user);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            LoadCartItems();
        }



        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HelpText_Click(object sender, MouseButtonEventArgs e)
        {
            var helpWindow = new HelpWindow(); // створимо це вікно окремо
            helpWindow.ShowDialog();
        }

    }
}
