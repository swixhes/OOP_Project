using DiscountMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace DiscountMarketplace.Domain
{
    public class RegisteredUser : User, IComparable<RegisteredUser>, IReviewManagement
    {
        public List<Order> PurchasedCoupons { get; private set; } = new List<Order>();
        private double balance;
        public int Id => base.Id;

        private static List<RegisteredUser> allUsers = new List<RegisteredUser>();

        public static RegisteredUser GetUserById(int id) =>
            allUsers.FirstOrDefault(u => u.Id == id);
        public double Balance
        {
            get => balance;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Баланс не може бути менше 0.");
                balance = value;
            }
        }

        public event EventHandler<CouponEventArgs> CouponReturned;
        public event EventHandler<string> Notification;

        private string password;
        public string Password
        {
            get => password;
            set => password = PasswordHasher.Hash(value);
        }
        public RegisteredUser(int id, string email, string firstName, string lastName, string phoneNumber, string password, double initialBalance)
    : base(id, email, firstName, lastName, phoneNumber)
        {
            ValidatePassword(password);
            Password = password;
            Balance = initialBalance;
            allUsers.Add(this);
        }

        public RegisteredUser(int id, string email, string firstName, string lastName, string phoneNumber, string hashedPassword, double initialBalance, bool isHashed)
            : base(id, email, firstName, lastName, phoneNumber)
        {
            if (isHashed)
                password = hashedPassword;
            else
            {
                ValidatePassword(hashedPassword);
                Password = hashedPassword;
            }
            Balance = initialBalance;
            allUsers.Add(this);
        }

        public override bool Login(string email, string password)
        {
            return Email == email && this.password == PasswordHasher.Hash(password);
        }
        public List<Order> ViewPurchasedCoupons()
        {
            return PurchasedCoupons.ToList();
        }

        public bool ReturnCoupon(int couponId)
        {
            var order = PurchasedCoupons
                .Where(o => o.Coupon.Id == couponId)
                .OrderByDescending(o => o.PurchaseDate)
                .FirstOrDefault(o => (DateTime.Now - o.PurchaseDate).TotalHours <= 1);

            if (order == null)
            {
                Notification?.Invoke(this, "Купон не знайдено серед придбаних або минув час повернення.");
                return false;
            }

            Balance += order.Coupon.Price;
            PurchasedCoupons.Remove(order);
            order.Coupon.RestoreUsage();
            CouponReturned?.Invoke(this, new CouponEventArgs(order.Coupon));
            //Notification?.Invoke(this, $"Купон {order.Coupon.Name} успішно повернено.");
            return true;
        }

        public int CompareTo(RegisteredUser? other)
        {
            if (other == null) return 1;
            return PurchasedCoupons.Count.CompareTo(other.PurchasedCoupons.Count);
        }

        public bool DeleteReview(int reviewId)
        {
            var allReviewsField = typeof(Review).GetField("allReviews", BindingFlags.NonPublic | BindingFlags.Static);
            if (allReviewsField?.GetValue(null) is List<Review> allReviewsList)
            {
                var review = allReviewsList.FirstOrDefault(r => r.Id == reviewId && r.Author == this);
                if (review != null)
                {
                    allReviewsList.Remove(review);
                    return true;
                }
            }
            return false;
        }

        public class CouponEventArgs : EventArgs
        {
            public Coupon Coupon { get; }

            public CouponEventArgs(Coupon coupon)
            {
                Coupon = coupon ?? throw new ArgumentNullException(nameof(coupon));
            }
        }
        public static List<RegisteredUser> GetAllUsers()
        {
            return allUsers;
        }

        public static int GenerateNewId()
        {
            return allUsers.Count > 0 ? allUsers.Max(u => u.ID) + 1 : 1;
        }
        public static void RegisterNewUser(string email, string firstName, string lastName, string phone, string password, string confirmPassword, string balanceText)
        {
            var errors = new Dictionary<string, string>();

            if (GetAllUsers().Any(u => u.Email == email))
                errors["Email"] = "Користувач із такою поштою вже існує.";

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || email.Length < 8 || email.Length > 20)
                errors["Email"] = "Електронна пошта має бути від 8 до 20 символів.";

            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > 20 || firstName.Any(char.IsDigit))
                errors["FirstName"] = "Ім'я не може бути порожнім, містити цифри і має бути не більше 20 символів.";

            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > 20 || lastName.Any(char.IsDigit))
                errors["LastName"] = "Прізвище не може бути порожнім, містити цифри і має бути не більше 20 символів.";

            if (string.IsNullOrWhiteSpace(phone) || !Regex.IsMatch(phone, @"^\+38\(0\d{2}\)-\d{7}$"))
                errors["Phone"] = "Невірний формат номера телефону. Очікується: +38(0XX)-XXXXXXX";

            // (validation bypassed)
            //if (string.IsNullOrWhiteSpace(password) || password.Length < 8 ||
            //    !password.Any(char.IsDigit) || !password.Any(c => "!@#$%^&*".Contains(c)))
            //    errors["Password"] = "Пароль має містити мінімум 8 символів, цифру та спецсимвол.";

            if (password != confirmPassword)
                errors["ConfirmPassword"] = "Паролі не співпадають.";

            if (!double.TryParse(balanceText, out double balance) || balance < 0 || email.Length > 5000)
                errors["Balance"] = "Баланс повинен бути числом більше або рівним 0.";

            if (errors.Any())
                throw new ValidationException(errors);

            var user = new RegisteredUser(GenerateNewId(), email, firstName, lastName, phone, password, balance);
            JsonStorage.SaveUsersToJson(allUsers);
        }
       
        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8 ||
                !password.Any(char.IsDigit) || !password.Any(c => "!@#$%^&*".Contains(c)))
                throw new ArgumentException("Пароль має містити мінімум 8 символів, цифру та спецсимвол.");
        }
        public bool TopUpBalance(double amount)
        {
            if (amount <= 0 || amount > 5000)
                return false;

            Balance += amount;
            return true;
        }
    }
}


