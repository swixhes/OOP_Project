using DiscountMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    [TestClass]
    public class GuestTests
    {
        [TestMethod]
        public void ViewCoupons_ShouldReturnAvailableCategoryCoupons()
        {
            var guest = new Guest(1, "a@a.com", "Ім'я", "Прізвище", "+38(050)-1234567");
            guest.AvailableCategories.Add(CouponCategory.Travel);

            var travelCoupon = new Coupon(1, "Trip", CouponCategory.Travel, DateTime.Now.AddDays(5), 20, 1);
            var foodCoupon = new Coupon(2, "Pizza", CouponCategory.Food, DateTime.Now.AddDays(5), 10, 1);

            var result = guest.ViewCoupons();

            Assert.IsFalse(result.Contains(foodCoupon));
        }

    }
}
