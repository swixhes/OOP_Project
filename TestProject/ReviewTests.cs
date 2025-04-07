using DiscountMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    [TestClass]
    public class ReviewTests
    {
        [TestMethod]
        public void Constructor_ValidData_AddsToList()
        {

            var user = new RegisteredUser(1, "a@a.com", "Ім'я", "Прізвище", "+38(050)-1234567", "pass", 0);
            var review = new Review(1, 10, user, 5, "Коментар");

            var reviews = Review.GetReviewsByCouponId(10);
            CollectionAssert.Contains(reviews, review);
        }
    }
}