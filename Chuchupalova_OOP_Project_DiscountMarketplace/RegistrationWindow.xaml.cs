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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
            HookTextChanged();
        }

        private void HookTextChanged()
        {
            EmailBox.TextChanged += (s, e) => HideError(EmailBox, EmailError);
            FirstNameBox.TextChanged += (s, e) => HideError(FirstNameBox, FirstNameError);
            LastNameBox.TextChanged += (s, e) => HideError(LastNameBox, LastNameError);
            PhoneBox.TextChanged += (s, e) => HideError(PhoneBox, PhoneError);
            PasswordBox.PasswordChanged += (s, e) => HideError(PasswordBox, PasswordError);
            ConfirmPasswordBox.PasswordChanged += (s, e) => HideError(ConfirmPasswordBox, ConfirmPasswordError);
            BalanceBox.TextChanged += (s, e) => HideError(BalanceBox, BalanceError);
        }
        private void HideError(Control control, TextBlock errorBlock)
        {
            control.ClearValue(Border.BorderBrushProperty);
            errorBlock.Text = "";
            errorBlock.Visibility = Visibility.Collapsed;
        }

        private bool isPasswordVisible = false;
        private bool isConfirmPasswordVisible = false;

        private void TogglePasswordVisibility(object sender, RoutedEventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                VisiblePasswordBox.Text = PasswordBox.Password;
                PasswordBox.Visibility = Visibility.Collapsed;
                VisiblePasswordBox.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordBox.Password = VisiblePasswordBox.Text;
                PasswordBox.Visibility = Visibility.Visible;
                VisiblePasswordBox.Visibility = Visibility.Collapsed;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!isPasswordVisible)
                VisiblePasswordBox.Text = PasswordBox.Password;
        }

        private void VisiblePasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isPasswordVisible)
                PasswordBox.Password = VisiblePasswordBox.Text;
        }

        private void ToggleConfirmPasswordVisibility(object sender, RoutedEventArgs e)
        {
            isConfirmPasswordVisible = !isConfirmPasswordVisible;

            if (isConfirmPasswordVisible)
            {
                VisibleConfirmPasswordBox.Text = ConfirmPasswordBox.Password;
                ConfirmPasswordBox.Visibility = Visibility.Collapsed;
                VisibleConfirmPasswordBox.Visibility = Visibility.Visible;
            }
            else
            {
                ConfirmPasswordBox.Password = VisibleConfirmPasswordBox.Text;
                ConfirmPasswordBox.Visibility = Visibility.Visible;
                VisibleConfirmPasswordBox.Visibility = Visibility.Collapsed;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!isConfirmPasswordVisible)
                VisibleConfirmPasswordBox.Text = ConfirmPasswordBox.Password;
        }

        private void VisibleConfirmPasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isConfirmPasswordVisible)
                ConfirmPasswordBox.Password = VisibleConfirmPasswordBox.Text;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            ClearErrors();

            string email = EmailBox.Text.Trim();
            string firstName = FirstNameBox.Text.Trim();
            string lastName = LastNameBox.Text.Trim();
            string phone = PhoneBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;
            string balanceText = BalanceBox.Text.Trim();

            try
            {
                RegisteredUser.RegisterNewUser(email, firstName, lastName, phone, password, confirmPassword, balanceText);
                var newUser = RegisteredUser.GetAllUsers().FirstOrDefault(u => u.Email == email);
                if (newUser != null)
                {
                    MessageBox.Show("Реєстрація успішна!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    var userWindow = new RegisteredUserWindow(newUser);
                    userWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Помилка при зчитуванні нового користувача.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (ValidationException vex)
            {
                foreach (var kvp in vex.Errors)
                {
                    switch (kvp.Key)
                    {
                        case "Email":
                            ShowError(EmailBox, EmailError, kvp.Value);
                            break;
                        case "FirstName":
                            ShowError(FirstNameBox, FirstNameError, kvp.Value);
                            break;
                        case "LastName":
                            ShowError(LastNameBox, LastNameError, kvp.Value);
                            break;
                        case "Phone":
                            ShowError(PhoneBox, PhoneError, kvp.Value);
                            break;
                        case "Password":
                            ShowError(PasswordBox, PasswordError, kvp.Value);
                            break;
                        case "ConfirmPassword":
                            ShowError(ConfirmPasswordBox, ConfirmPasswordError, kvp.Value);
                            break;
                        case "Balance":
                            ShowError(BalanceBox, BalanceError, kvp.Value);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Невідома помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowExceptionError(Exception ex)
        {
            if (ex.Message.Contains("пошта"))
                ShowError(EmailBox, EmailError, ex.Message);
            else if (ex.Message.Contains("Ім'я"))
                ShowError(FirstNameBox, FirstNameError, ex.Message);
            else if (ex.Message.Contains("Прізвище"))
                ShowError(LastNameBox, LastNameError, ex.Message);
            else if (ex.Message.Contains("номер телефону"))
                ShowError(PhoneBox, PhoneError, ex.Message);
            else if (ex.Message.Contains("Пароль"))
                ShowError(PasswordBox, PasswordError, ex.Message);
            else if (ex.Message.Contains("Баланс"))
                ShowError(BalanceBox, BalanceError, ex.Message);
            else
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowError(Control control, TextBlock errorBlock, string message)
        {
            control.BorderBrush = System.Windows.Media.Brushes.Red;
            errorBlock.Text = message;
            errorBlock.Visibility = Visibility.Visible;
        }

        private void ClearErrors()
        {
            foreach (var textBlock in new[] { EmailError, FirstNameError, LastNameError, PhoneError, PasswordError, ConfirmPasswordError, BalanceError })
            {
                textBlock.Visibility = Visibility.Collapsed;
                textBlock.Text = "";
            }

            foreach (var control in new Control[] { EmailBox, FirstNameBox, LastNameBox, PhoneBox, PasswordBox, ConfirmPasswordBox, BalanceBox })
            {
                control.ClearValue(Border.BorderBrushProperty);
            }
        }

        private void LoginLink_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var loginWindow = new LoginPage();
            this.Close();
            loginWindow.Show();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == EmailBox)
                EmailPlaceholder.Visibility = string.IsNullOrEmpty(EmailBox.Text) ? Visibility.Visible : Visibility.Collapsed;
            else if (sender == FirstNameBox)
                FirstNamePlaceholder.Visibility = string.IsNullOrEmpty(FirstNameBox.Text) ? Visibility.Visible : Visibility.Collapsed;
            else if (sender == LastNameBox)
                LastNamePlaceholder.Visibility = string.IsNullOrEmpty(LastNameBox.Text) ? Visibility.Visible : Visibility.Collapsed;
            else if (sender == PhoneBox)
                PhonePlaceholder.Visibility = string.IsNullOrEmpty(PhoneBox.Text) ? Visibility.Visible : Visibility.Collapsed;
            else if (sender == BalanceBox)
                BalancePlaceholder.Visibility = string.IsNullOrEmpty(BalanceBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginPage();
            this.Close();
            loginWindow.Show();
        }
    }
}
