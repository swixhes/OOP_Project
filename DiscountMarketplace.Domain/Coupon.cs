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
            if (coupon != null && !allCoupons.Any(c => c.Id == coupon.Id))
                allCoupons.Add(coupon);
        }

        public static List<Coupon> GetAllCoupons() => allCoupons;

        public int Id { get; }
        public string Name { get; }
        public CouponCategory Category { get; }
        public DateTime ExpirationDate { get; }
        public double Discount { get; }
        public int UsageLimit { get; private set; }

        public double Price => 100.0 * (1 - Discount / 100.0); // додано властивість Price

        public Coupon(int id, string name, CouponCategory category, DateTime expirationDate, double discount, int usageLimit, bool allowPastExpiration = false)
        {
            if (id <= 0) throw new ArgumentException("ID має бути більше 0.");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Назва не може бути порожньою.");
            if (!allowPastExpiration && expirationDate <= DateTime.Now) throw new ArgumentException("Дата закінчення дії не може бути в минулому.");
            if (discount < 0 || discount > 100) throw new ArgumentException("Знижка має бути від 0 до 100%.");
            if (usageLimit <= 0) throw new ArgumentException("Ліміт використань має бути більше 0.");

            Id = id;
            Name = name;
            Category = category;
            ExpirationDate = expirationDate;
            Discount = discount;
            UsageLimit = usageLimit;

            AddCoupon(this);
        }

        public bool IsValid() => ExpirationDate > DateTime.Now && UsageLimit > 0;

        public bool MarkAsUsed()
        {
            if (IsValid())
            {
                UsageLimit--;
                return true;
            }
            return false;
        }

    }
}


