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

    public class SerializableCoupon
    {
        public int CouponId { get; set; }
        public string Category { get; set; }
    }
    public static class JsonStorage
    {
        private static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.json");


        public static void SaveUsersToJson(List<RegisteredUser> users)
        {
            var data = users.Select(u => new SerializableRegisteredUser
            {
                Id = u.ID,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                Password = u.Password, // Треба надати public геттер
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
    }

}
