using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiscountMarketplace.Domain
{
    public class SerializableUser
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<SerializableCoupon> PurchasedCoupons { get; set; }
    }

    public class SerializableCoupon
    {
        public int CouponId { get; set; }
        public string Category { get; set; }
    }
    public static class JsonStorage
    {
        public static void SaveUsersToJson(List<RegisteredUser> users, string filePath)
        {
            var data = users.Select((u, i) => new SerializableUser
            {
                Id = i,
                FullName = u.LastName + " " + u.FirstName,
                PurchasedCoupons = u.PurchasedCoupons.Select((o, index) => new SerializableCoupon
                {
                    CouponId = index,
                    Category = o.Coupon.Category.ToString()
                }).ToList()
            }).ToList();

            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static List<SerializableUser> LoadUsersFromJson(string filePath)
        {
            if (!File.Exists(filePath)) return new List<SerializableUser>();

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<SerializableUser>>(json);
        }
    }

}
