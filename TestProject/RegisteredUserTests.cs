using DiscountMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    [TestClass]
    public class RegisteredUserTests
    {
        [TestMethod]
        public void Login_ValidCredentials_ShouldReturnTrue()
        {
            // Arrange
            var user = new RegisteredUser(1, "user@mail.com", "Анна", "Іванова", "+38(050)-9876543", "pass123", 50);
            bool notificationTriggered = false;
            user.Notification += (s, m) => notificationTriggered = true;

            // Act
            bool result = user.Login("user@mail.com", "pass123");

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(notificationTriggered);
        }

        [TestMethod]
        public void PurchaseCoupon_ValidCoupon_ShouldSucceed()
        {
            // Arrange
            var coupon = new Coupon(1, "Знижка 20%", CouponCategory.Food, DateTime.Now.AddDays(5), 30, 20);
            var user = new RegisteredUser(2, "shopper@mail.com", "Олег", "Коваль", "+38(093)-1122334", "pass", 100);

            // Act
            bool result = user.PurchaseCoupon(coupon);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, user.PurchasedCoupons.Count);
        }

        [TestMethod]
        public void ReturnCoupon_Valid_ShouldRestoreBalance()
        {
            // Arrange
            var coupon = new Coupon(2, "Карпати", CouponCategory.Travel, DateTime.Now.AddDays(5), 40, 10);
            var user = new RegisteredUser(3, "reader@mail.com", "Марія", "Левченко", "+38(096)-4455667", "123", 100);
            user.PurchaseCoupon(coupon);
            double balanceBefore = user.Balance;

            // Act
            bool result = user.ReturnCoupon(coupon.Id);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(balanceBefore + coupon.Price, user.Balance);
            Assert.AreEqual(0, user.PurchasedCoupons.Count);
        }

        [TestMethod]
        public void Balance_SetNegative_ShouldThrow()
        {
            // Arrange
            var user = new RegisteredUser(4, "test@mail.com", "Ірина", "Мельник", "+38(063)-9988776", "pass", 100);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => user.Balance = -10);
        }
        [TestMethod]
        public void DeleteReview_ValidReviewOwnedByUser_ShouldRemoveReview()
        {
            // Arrange
            var user = new RegisteredUser(1, "test@test.com", "Іван", "Іванов", "+38(050)-1234567", "pass123", 100);
            var review = new Review(1, 100, user, 5, "Гарно");

            // Act
            bool result = user.DeleteReview(1);

            // Assert
            Assert.IsTrue(result);
            var reviews = Review.GetReviewsByCouponId(100);
            Assert.IsFalse(reviews.Contains(review));
        }

        [TestMethod]
        public void DeleteReview_InvalidReviewId_ShouldReturnFalse()
        {
            // Arrange
            var user = new RegisteredUser(1, "test@test.com", "Іван", "Іванов", "+38(050)-1234567", "pass123", 100);

            // Act
            bool result = user.DeleteReview(999);

            // Assert
            Assert.IsFalse(result);
        }

    }

}
