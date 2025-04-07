using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountMarketplace.Domain
{
    public class Guest : User
    {
        public List<CouponCategory> AvailableCategories { get; } = new List<CouponCategory>();

        public Guest(int id, string email, string firstName, string lastName, string phoneNumber)
            : base(id, email, firstName, lastName, phoneNumber) { }

        public override bool Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public List<Coupon> ViewCoupons()
        {
            throw new NotImplementedException();
        }

        public List<Review> ViewReviews(int couponId)
        {
            throw new NotImplementedException();
        }
    }
}
