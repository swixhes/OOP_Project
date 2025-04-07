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
            return false;
        }

        public List<Coupon> ViewCoupons()
        {
            return Coupon.GetAllCoupons()
                         .Where(c => AvailableCategories.Contains(c.Category))
                         .ToList();
        }

        public List<Review> ViewReviews(int couponId)
        {
            return Review.GetReviewsByCouponId(couponId);
        }
    }
}
