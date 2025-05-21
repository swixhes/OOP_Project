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

namespace Chuchupalova_OOP_Project_DiscountMarketplace
{
    /// <summary>
    /// Логика взаимодействия для SuccessfulRegistration.xaml
    /// </summary>
    public partial class SuccessfulRegistration : Window
    {
        private RegisteredUser newUser;

        public SuccessfulRegistration(RegisteredUser user)
        {
            InitializeComponent();
            newUser = user;
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            var userWindow = new RegisteredUserWindow(newUser);
            userWindow.Show();
            this.Close();
        }
    }
}
