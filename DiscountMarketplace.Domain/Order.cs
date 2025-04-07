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


        public Order(RegisteredUser user, Coupon coupon, DateTime purchaseDate)
        {
            throw new NotImplementedException();
        }

        public bool ConfirmPurchase()
        {
            throw new NotImplementedException();
        }
    }
}

