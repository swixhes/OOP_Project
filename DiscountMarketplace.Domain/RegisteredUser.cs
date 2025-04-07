using DiscountMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiscountMarketplace.Domain
{
    public delegate void PurchaseHandler(string message);
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

        public event EventHandler<CouponEventArgs> CouponPurchased;
        public event EventHandler<CouponEventArgs> CouponReturned;
        public event EventHandler<string> Notification;
        public event PurchaseHandler OnSuccessfulPurchase;

        private string password;

        public RegisteredUser(int id, string email, string firstName, string lastName, string phoneNumber, string password, double initialBalance)
            : base(id, email, firstName, lastName, phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Пароль не може бути порожнім.");

            this.password = password;
            Balance = initialBalance;
            allUsers.Add(this);
        }

        public override bool Login(string email, string password)
        {
            if (this.Email == email && this.password == password)
            {
                Notification?.Invoke(this, $"{FirstName} {LastName} увійшов в систему.");
                return true;
            }

            Notification?.Invoke(this, "Неправильний email або пароль.");
            return false;
        }

        public bool PurchaseCoupon(Coupon coupon)
        {
            if (coupon == null)
                throw new ArgumentNullException(nameof(coupon));

            if (!coupon.IsValid())
            {
                Notification?.Invoke(this, "Купон недійсний або термін дії закінчився.");
                return false;
            }

            if (Balance < coupon.Price)
            {
                Notification?.Invoke(this, "Недостатньо коштів на балансі.");
                return false;
            }

            Balance -= coupon.Price;
            var order = new Order(this, coupon, DateTime.Now);
            PurchasedCoupons.Add(order);
            coupon.MarkAsUsed();
            CouponPurchased?.Invoke(this, new CouponEventArgs(coupon));
            OnSuccessfulPurchase?.Invoke($"Купон {coupon.Name} успішно придбано.");
            return true;
        }

        public List<Order> ViewPurchasedCoupons()
        {
            return PurchasedCoupons.ToList();
        }

        public bool ReturnCoupon(int couponId)
        {
            var order = PurchasedCoupons.FirstOrDefault(o => o.Coupon.Id == couponId);
            if (order == null)
            {
                Notification?.Invoke(this, "Купон не знайдено серед придбаних.");
                return false;
            }

            if (!order.Coupon.IsValid())
            {
                Notification?.Invoke(this, "Купон вже використано і не може бути повернутий.");
                return false;
            }

            Balance += order.Coupon.Price;
            PurchasedCoupons.Remove(order);
            CouponReturned?.Invoke(this, new CouponEventArgs(order.Coupon));
            Notification?.Invoke(this, $"Купон {order.Coupon.Name} успішно повернено.");
            return true;
        }

        public int CompareTo(RegisteredUser other)
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

    }
}


