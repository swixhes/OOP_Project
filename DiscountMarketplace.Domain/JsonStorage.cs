using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiscountMarketplace.Domain
{
    public class SerializableRegisteredUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public double Balance { get; set; }
    }

    public class SerializableOrder
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int CouponId { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
    public class SerializableCoupon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CouponCategory Category { get; set; }
        public DateTime ExpirationDate { get; set; }
        public double Discount { get; set; }
        public int UsageLimit { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int PurchaseCount { get; set; }
        public string ImagePath { get; set; }
    }

    public class SerializableReview
    {
        public int Id { get; set; }
        public int CouponId { get; set; }
        public int AuthorId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
    public static class JsonStorage
    {
        private static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.json");
        private static string ordersFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "orders.json");
        private static string couponsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "coupons.json");
        private static string reviewsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reviews.json");

        public static void SaveUsersToJson(List<RegisteredUser> users)
        {
            var data = users.Select(u => new SerializableRegisteredUser
            {
                Id = u.ID,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                Password = u.Password,
                Balance = u.Balance
            }).ToList();

            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static void LoadUsersFromJson()
        {
            if (!File.Exists(filePath))
                return;

            string json = File.ReadAllText(filePath);
            var users = JsonSerializer.Deserialize<List<SerializableRegisteredUser>>(json);

            if (users != null)
            {
                foreach (var u in users)
                {
                    new RegisteredUser(u.Id, u.Email, u.FirstName, u.LastName, u.PhoneNumber, u.Password, u.Balance, true);
                }
            }
        }
        public static void SaveOrdersToJson(List<Order> orders)
        {
            var data = orders.Select(o => new SerializableOrder
            {
                OrderId = o.Id,
                UserId = o.User.ID,
                CouponId = o.Coupon.Id,
                PurchaseDate = o.PurchaseDate
            }).ToList();

            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ordersFilePath, json);
        }

        public static void LoadOrdersFromJson(List<RegisteredUser> users, List<Coupon> coupons)
        {
            if (!File.Exists(ordersFilePath))
                return;
            foreach (var user in users)
            {
                user.PurchasedCoupons.Clear();
            }
            string json = File.ReadAllText(ordersFilePath);
            var orders = JsonSerializer.Deserialize<List<SerializableOrder>>(json);

            if (orders != null)
            {
                foreach (var so in orders)
                {
                    var user = users.FirstOrDefault(u => u.ID == so.UserId);
                    var coupon = coupons.FirstOrDefault(c => c.Id == so.CouponId);

                    if (user != null && coupon != null)
                    {
                        var order = new Order(user, coupon, so.PurchaseDate);
                        user.PurchasedCoupons.Add(order);
                    }
                }
            }
        }
        public static void SaveCouponsToJson(List<Coupon> coupons)
        {
            var data = coupons.Select(c => new SerializableCoupon
            {
                Id = c.Id,
                Name = c.Name,
                Price = c.Price,
                Discount = c.Discount,
                ExpirationDate = c.ExpirationDate,
                Category = c.Category,
                UsageLimit = c.UsageLimit,
                PurchaseCount = c.PurchaseCount,
                Description = c.Description,
                ImagePath = c.ImagePath
            }).ToList();

            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(couponsFilePath, json);
        }

        public static List<Coupon> LoadCouponsFromJson()
        {
            if (!File.Exists(couponsFilePath))
                return new List<Coupon>();

            string json = File.ReadAllText(couponsFilePath);
            var coupons = JsonSerializer.Deserialize<List<SerializableCoupon>>(json);

            var loadedCoupons = coupons.Select(c =>
            {
                var coupon = new Coupon(c.Id, c.Name, c.Category, c.ExpirationDate, c.Discount, c.UsageLimit, c.ImagePath, c.Description);
                typeof(Coupon).GetProperty("PurchaseCount").SetValue(coupon, c.PurchaseCount);
                return coupon;
            }).ToList();

            // Оновлюємо allCoupons
            typeof(Coupon).GetField("allCoupons", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, loadedCoupons);

            return loadedCoupons;
        }

        public static void SaveReviewsToJson(List<Review> reviews)
        {
            var data = reviews.Select(r => new SerializableReview
            {
                Id = r.Id,
                CouponId = r.CouponId,
                AuthorId = r.Author.ID,
                Rating = r.Rating,
                Comment = r.Comment
            }).ToList();

            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(reviewsFilePath, json);
        }

        public static void LoadReviewsFromJson(List<RegisteredUser> users)
        {
            Review.ClearAllReviews();
            if (!File.Exists(reviewsFilePath))
                return;

            string json = File.ReadAllText(reviewsFilePath);
            var reviews = JsonSerializer.Deserialize<List<SerializableReview>>(json);
            
            if (reviews != null)
            {
                foreach (var r in reviews)
                {
                    var author = users.FirstOrDefault(u => u.ID == r.AuthorId);
                    if (author != null)
                    {
                        var review = new Review(r.CouponId, author, r.Rating, r.Comment);
                        typeof(Review).GetField("nextId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                            ?.SetValue(null, Math.Max(r.Id + 1, (int)typeof(Review).GetField("nextId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)?.GetValue(null)));
                    }
                }
            }
        }
    }
}
