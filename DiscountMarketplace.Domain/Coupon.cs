using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountMarketplace.Domain
{
    public class Coupon
    {
        private static List<Coupon> allCoupons = new List<Coupon>();

        public static void AddCoupon(Coupon coupon)
        {
            throw new NotImplementedException();
        }

        public static List<Coupon> GetAllCoupons() => allCoupons;

        public int Id { get; }
        public string Name { get; }
        public CouponCategory Category { get; }
        public DateTime ExpirationDate { get; }
        public double Discount { get; }
        public int UsageLimit { get; private set; }

        public double Price => 100.0 * (1 - Discount / 100.0);

        public Coupon(int id, string name, CouponCategory category, DateTime expirationDate, double discount, int usageLimit, bool allowPastExpiration = false)
        {
            throw new NotImplementedException();
        }

        public bool IsValid() => ExpirationDate > DateTime.Now && UsageLimit > 0;

        public bool MarkAsUsed()
        {
            throw new NotImplementedException();
        }

    }
}

