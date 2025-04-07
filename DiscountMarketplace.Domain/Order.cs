using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountMarketplace.Domain
{
    public class Order
    {
        private static int nextId = 1;

        public int Id { get; }
        public RegisteredUser User { get; }
        public Coupon Coupon { get; }
        public DateTime PurchaseDate { get; }

        public Coupon Coupons
        {
            get => default;
            set
            {
            }
        }

        public Order(RegisteredUser user, Coupon coupon, DateTime purchaseDate)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (coupon == null || !coupon.IsValid()) throw new ArgumentException("Купон має бути дійсним.");
            if (purchaseDate > DateTime.Now) throw new ArgumentException("Дата покупки не може бути в майбутньому.");

            Id = nextId++;
            User = user;
            Coupon = coupon;
            PurchaseDate = purchaseDate;
        }

        public bool ConfirmPurchase()
        {
            if (Coupon.MarkAsUsed())
            {
                User.PurchasedCoupons.Add(this);
                return true;
            }
            return false;
        }
    }
}

