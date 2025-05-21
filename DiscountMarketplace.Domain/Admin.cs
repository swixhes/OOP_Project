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
        public bool CreateOrEditCoupon(Coupon updatedCoupon)
        {
            if (updatedCoupon == null)
                throw new ArgumentNullException(nameof(updatedCoupon));

            var allField = typeof(Coupon).GetField("allCoupons", BindingFlags.NonPublic | BindingFlags.Static);
            if (allField?.GetValue(null) is List<Coupon> all)
            {
                var existing = all.FirstOrDefault(c => c.Id == updatedCoupon.Id);
                if (existing != null)
                {
                    existing.Name = updatedCoupon.Name;
                    existing.Category = updatedCoupon.Category;
                    existing.ExpirationDate = updatedCoupon.ExpirationDate;
                    existing.Discount = updatedCoupon.Discount;
                    existing.UsageLimit = updatedCoupon.UsageLimit;
                    existing.Description = updatedCoupon.Description;
                }
                else
                {
                    all.Add(updatedCoupon);
                }

                JsonStorage.SaveCouponsToJson(all);
                return true;
            }
            return false;
        }
    }
}