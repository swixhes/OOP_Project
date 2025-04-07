using DiscountMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
        public void Constructor_ValidData_ShouldCreateOrder()
        {
            var user = new RegisteredUser(1, "a@a.com", "Ім'я", "Прізвище", "+38(050)-1234567", "pass", 100);
            var coupon = new Coupon(1, "Test", CouponCategory.Food, DateTime.Now.AddDays(1), 10, 1);
            var order = new Order(user, coupon, DateTime.Now);

            Assert.AreEqual(user, order.User);
            Assert.AreEqual(coupon, order.Coupon);
        }

        [TestMethod]
        public void ConfirmPurchase_Valid_ShouldMarkUsed()
        {
            var user = new RegisteredUser(1, "a@a.com", "Ім'я", "Прізвище", "+38(050)-1234567", "pass", 100);
            var coupon = new Coupon(2, "Test", CouponCategory.Food, DateTime.Now.AddDays(1), 10, 1);
            var order = new Order(user, coupon, DateTime.Now);

            bool result = order.ConfirmPurchase();

            Assert.IsTrue(result);
            Assert.AreEqual(0, coupon.UsageLimit);
        }

    }
}
