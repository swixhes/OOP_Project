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
            if (review != null && !allReviews.Any(r => r.Id == review.Id))
                allReviews.Add(review);
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
            if (id <= 0) throw new ArgumentException("ID має бути більше 0.");
            if (author == null) throw new ArgumentNullException(nameof(author));
            if (rating < 1 || rating > 5) throw new ArgumentException("Оцінка має бути від 1 до 5.");

            Id = id;
            CouponId = couponId;
            Author = author;
            Rating = rating;
            Comment = comment;

            AddReview(this);
        }
    }
}
