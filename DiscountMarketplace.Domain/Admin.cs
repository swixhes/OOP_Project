using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscountMarketplace.Domain
{
    public class Admin : User, IReviewManagement, ICouponManagement
    {
        private List<RegisteredUser> blockedUsers = new List<RegisteredUser>();

        public IReadOnlyList<RegisteredUser> BlockedUsers => blockedUsers.AsReadOnly();
        public event EventHandler<string> Notification;

        private string password;
        public string Password => password;

        public Admin(int id, string email, string firstName, string lastName, string phoneNumber, string password)
            : base(id, email, firstName, lastName, phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Пароль не може бути порожнім.");
            this.password = password;
        }

        public override bool Login(string email, string password)
        {
            if (this.Email == email && this.Password == password)
            {
                Notification?.Invoke(this, $"Адмін {FirstName} {LastName} увійшов в систему.");
                return true;
            }

            Notification?.Invoke(this, "Неправильний email або пароль.");
            return false;
        }

        public bool DeleteReview(int reviewId)
        {
            var allReviewsField = typeof(Review).GetField("allReviews", BindingFlags.NonPublic | BindingFlags.Static);
            if (allReviewsField?.GetValue(null) is List<Review> allReviewsList)
            {
                var review = allReviewsList.FirstOrDefault(r => r.Id == reviewId);
                if (review != null)
                {
                    allReviewsList.Remove(review);
                    return true;
                }
            }
            return false;
        }

        public bool BlockUser(int userId)
        {
            var user = RegisteredUser.GetUserById(userId);
            if (user != null && !blockedUsers.Contains(user))
            {
                blockedUsers.Add(user);
                return true;
            }
            return false;
        }

        public bool UnblockUser(int userId)
        {
            var user = blockedUsers.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                blockedUsers.Remove(user);
                return true;
            }
            return false;
        }

        public bool CreateDiscount(int couponId, double discountPercentage)
        {
            var coupon = Coupon.GetAllCoupons().FirstOrDefault(c => c.Id == couponId);
            if (coupon != null && coupon.IsValid() && discountPercentage >= 0 && discountPercentage <= 100)
            {
                var discountProperty = typeof(Coupon).GetProperty("Discount", BindingFlags.Public | BindingFlags.Instance);
                if (discountProperty != null && discountProperty.CanWrite)
                {
                    discountProperty.SetValue(coupon, discountPercentage);
                    return true;
                }
            }
            return false;
        }

        public bool EditCoupon(int couponId, Coupon updatedCoupon)
        {
            var allField = typeof(Coupon).GetField("allCoupons", BindingFlags.NonPublic | BindingFlags.Static);
            if (allField?.GetValue(null) is List<Coupon> all)
            {
                var existing = all.FirstOrDefault(c => c.Id == couponId);
                if (existing != null)
                {
                    all.Remove(existing);
                    all.Add(updatedCoupon);
                    return true;
                }
            }
            return false;
        }

        public bool DeleteCoupon(int couponId)
        {
            var allField = typeof(Coupon).GetField("allCoupons", BindingFlags.NonPublic | BindingFlags.Static);
            if (allField?.GetValue(null) is List<Coupon> allCoupons)
            {
                var coupon = allCoupons.FirstOrDefault(c => c.Id == couponId);
                if (coupon != null)
                {
                    allCoupons.Remove(coupon);
                    return true;
                }
            }
            return false;
        }
    }
}