using DiscountMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class Review
{
    private static List<Review> allReviews = new List<Review>();
    private static int nextId = 1;

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

    public Review(int couponId, RegisteredUser author, int rating, string comment = "")
    {
        if (couponId <= 0) throw new ArgumentException("Невірний ID купона.");
        if (author == null) throw new ArgumentNullException(nameof(author));
        if (rating < 1 || rating > 5) throw new ArgumentException("Оцінка має бути від 1 до 5.");

        Id = nextId++;
        CouponId = couponId;
        Author = author;
        Rating = rating;
        Comment = comment;

        AddReview(this);
    }

    public static double GetAverageRating(int couponId)
    {
        var reviews = GetReviewsByCouponId(couponId);
        return reviews.Any() ? reviews.Average(r => r.Rating) : 0;
    }
}
