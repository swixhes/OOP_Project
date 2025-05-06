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
            var coupon = new Coupon(1, "Знижка 20%", CouponCategory.Food, DateTime.Now.AddDays(5), 30, 20, "Images/beauty_coupon.jpg", "Соковита піца з сиром та ковбаскою – зі знижкою 20%!");

            bool result = coupon.MarkAsUsed();

            Assert.IsTrue(result);
            Assert.AreEqual(1, coupon.UsageLimit);
        }

        [TestMethod]
        public void IsValid_Expired_ReturnsFalse()
        {
            var coupon = new Coupon(1, "Знижка 20%", CouponCategory.Food, DateTime.Now.AddDays(5), 30, 20, "Images/beauty_coupon.jpg", "Соковита піца з сиром та ковбаскою – зі знижкою 20%!");

            Assert.IsFalse(coupon.IsValid());

        }

    }
}
