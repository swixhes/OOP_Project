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
        private static Dictionary<RegisteredUser, List<Coupon>> cart = new();

        public int Id { get; }
        public RegisteredUser User { get; }
        public Coupon Coupon { get; }
        public DateTime PurchaseDate { get; }

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

        // CartManager logic
        public static void AddToCart(RegisteredUser user, Coupon coupon)
        {
            if (!cart.ContainsKey(user))
                cart[user] = new List<Coupon>();
            if (!cart[user].Contains(coupon))
                cart[user].Add(coupon);
        }

        public static List<Coupon> GetCart(RegisteredUser user)
        {
            return cart.ContainsKey(user) ? new List<Coupon>(cart[user]) : new List<Coupon>();
        }

        public static void ClearCart(RegisteredUser user)
        {
            if (cart.ContainsKey(user))
                cart[user].Clear();
        }

        public static bool IsInCart(RegisteredUser user, Coupon coupon)
        {
            return cart.ContainsKey(user) && cart[user].Contains(coupon);
        }
    }
}

