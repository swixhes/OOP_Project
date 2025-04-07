using DiscountMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    [TestClass]
    public class CouponTests
    {
        [TestMethod]
        public void MarkAsUsed_Valid_ShouldDecreaseUsageLimit()
        {
            var coupon = new Coupon(1, "Test", CouponCategory.Sport, DateTime.Now.AddDays(1), 15, 2);

            bool result = coupon.MarkAsUsed();

            Assert.IsTrue(result);
            Assert.AreEqual(1, coupon.UsageLimit);
        }

        [TestMethod]
        public void IsValid_Expired_ReturnsFalse()
        {
            var coupon = new Coupon(2, "Expired", CouponCategory.Food, DateTime.Now.AddDays(-1), 10, 1, true);

            Assert.IsFalse(coupon.IsValid());

        }

    }
}
