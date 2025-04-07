using DiscountMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Login_ShouldReturnTrue_WhenCredentialsAreCorrect()
        {
            var admin = new Admin(1, "admin@a.com", "Test", "Admin", "+38(050)-1111111", "admin123");
            bool result = admin.Login("admin@a.com", "admin123");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Login_ShouldReturnFalse_WhenCredentialsAreIncorrect()
        {
            var admin = new Admin(1, "admin@a.com", "Test", "Admin", "+38(050)-1111111", "admin123");
            bool result = admin.Login("admin@a.com", "wrongpass");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DeleteReview_ShouldRemoveReview()
        {
            var admin = new Admin(1, "admin@a.com", "Адмін", "Адмінович", "+38(050)-1234567", "admin123");
            var user = new RegisteredUser(2, "user@a.com", "Ім'я", "Прізвище", "+38(050)-1234567", "pass", 100);
            var review = new Review(1, 1, user, 5, "Відгук");

            bool result = admin.DeleteReview(1);

            Assert.IsTrue(result);
            var reviews = Review.GetReviewsByCouponId(1);
            Assert.IsFalse(reviews.Contains(review));
        }
        [TestMethod]
        public void BlockUser_ShouldAddUserToBlockedList()
        {
            var admin = new Admin(1, "admin@a.com", "Адмін", "Адмінович", "+38(050)-1234567", "admin123");
            var user = new RegisteredUser(2, "user@a.com", "Ім'я", "Прізвище", "+38(050)-1234567", "pass", 100);

            bool result = admin.BlockUser(user.Id);

            Assert.IsTrue(result);
            Assert.IsTrue(admin.BlockedUsers.Contains(user));
        }

        [TestMethod]
        public void UnblockUser_ShouldRemoveUserFromBlockedList()
        {
            var admin = new Admin(1, "admin@a.com", "Адмін", "Адмінович", "+38(050)-1234567", "admin123");
            var user = new RegisteredUser(2, "user@a.com", "Ім'я", "Прізвище", "+38(050)-1234567", "pass", 100);
            admin.BlockUser(user.Id);

            bool result = admin.UnblockUser(user.Id);

            Assert.IsTrue(result);
            Assert.IsFalse(admin.BlockedUsers.Contains(user));
        }

        [TestMethod]
        public void EditCoupon_ShouldReplaceCoupon()
        {
            var admin = new Admin(1, "admin@a.com", "Test", "Admin", "+38(050)-1111111", "admin123");
            var original = new Coupon(1, "Old", CouponCategory.Food, DateTime.Now.AddDays(5), 10, 5);
            var updated = new Coupon(1, "New", CouponCategory.Food, DateTime.Now.AddDays(5), 50, 5);

            bool result = admin.EditCoupon(1, updated);

            Assert.IsTrue(result);
            var all = Coupon.GetAllCoupons();
            Assert.IsTrue(all.Any(c => c.Name == "New" && c.Id == 1));
        }
        [TestMethod]
        public void DeleteCoupon_ShouldRemoveCoupon()
        {
            var admin = new Admin(1, "admin@a.com", "Test", "Admin", "+38(050)-1111111", "admin123");
            var coupon = new Coupon(1, "ToDelete", CouponCategory.Food, DateTime.Now.AddDays(5), 10, 5);

            bool result = admin.DeleteCoupon(1);

            Assert.IsTrue(result);
            Assert.IsFalse(Coupon.GetAllCoupons().Any(c => c.Id == 1));
        }

    }
}
