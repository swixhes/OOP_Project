using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscountMarketplace.Domain
{
    public class Review
    {
        private static List<Review> allReviews = new List<Review>();

        public static void AddReview(Review review)
        {
            throw new NotImplementedException();
        }

        public static List<Review> GetReviewsByCouponId(int couponId) =>
            allReviews.Where(r => r.CouponId == couponId).ToList();

        public int Id { get; }
        public int CouponId { get; }
        public RegisteredUser Author { get; }
        public int Rating { get; private set; }
        public string Comment { get; }
        private bool ratingUpdated = false;

        public Review(int id, int couponId, RegisteredUser author, int rating, string comment = "")
        {
            throw new NotImplementedException();
        }
    }
}
