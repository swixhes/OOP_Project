using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiscountMarketplace.Domain
{
    public class Order
    {
        private static int nextId = 1;
        private static Dictionary<RegisteredUser, List<CartItemModel>> cart = new();

        public int Id { get; set; }
        public RegisteredUser User { get; set; }
        public Coupon Coupon { get; set; }
        public DateTime PurchaseDate { get; set; }

        public Order() { }

        public Order(RegisteredUser user, Coupon coupon, DateTime purchaseDate)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (coupon == null /*|| !coupon.IsValid()*/) throw new ArgumentException("Купон має бути дійсним.");
            if (purchaseDate > DateTime.Now) throw new ArgumentException("Дата покупки не може бути в майбутньому.");

            Id = nextId++;
            User = user;
            Coupon = coupon;
            PurchaseDate = purchaseDate;
        }

        public bool ConfirmPurchase()
        {
            var actualCoupon = Coupon.GetAllCoupons().FirstOrDefault(c => c.Id == Coupon.Id);
            if (actualCoupon == null) return false;

            if (actualCoupon.MarkAsUsed())
            {
                User.PurchasedCoupons.Add(this);
                JsonStorage.SaveOrdersToJson(RegisteredUser.GetAllUsers().SelectMany(u => u.ViewPurchasedCoupons()).ToList());
                JsonStorage.SaveUsersToJson(RegisteredUser.GetAllUsers());
                JsonStorage.SaveCouponsToJson(Coupon.GetAllCoupons());
                return true;
            }
            return false;
        }


        // CartManager logic
        public static void AddToCart(RegisteredUser user, Coupon coupon)
        {
            var actualCoupon = Coupon.GetAllCoupons().FirstOrDefault(c => c.Id == coupon.Id);
            if (actualCoupon == null)
                throw new ArgumentException("Купон не знайдено.");

            // Додана перевірка на дійсність
            if (!actualCoupon.IsValid())
                throw new InvalidOperationException($"Купон \"{actualCoupon.Name}\" більше не дійсний і не може бути доданий до кошика.");

            if (!cart.ContainsKey(user))
                cart[user] = new List<CartItemModel>();

            var item = cart[user].FirstOrDefault(ci => ci.Coupon.Id == actualCoupon.Id);
            if (item != null)
                item.Quantity++;
            else
                cart[user].Add(new CartItemModel { Coupon = actualCoupon, Quantity = 1 });
        }


        public static List<CartItemModel> GetCart(RegisteredUser user)
        {
            return cart.ContainsKey(user) ? new List<CartItemModel>(cart[user]) : new List<CartItemModel>();
        }

        public static void ClearCart(RegisteredUser user)
        {
            if (cart.ContainsKey(user))
                cart[user].Clear();
        }

        public static void RemoveFromCart(RegisteredUser user, Coupon coupon)
        {
            if (!cart.ContainsKey(user)) return;

            var item = cart[user].FirstOrDefault(ci => ci.Coupon.Id == coupon.Id);
            if (item != null)
                cart[user].Remove(item);
        }

        public static void IncreaseQuantity(RegisteredUser user, Coupon coupon)
        {
            if (!cart.ContainsKey(user)) return;

            var item = cart[user].FirstOrDefault(ci => ci.Coupon.Id == coupon.Id);
            if (item != null)
                item.Quantity++;
        }

        public static void DecreaseQuantity(RegisteredUser user, Coupon coupon)
        {
            if (!cart.ContainsKey(user)) return;

            var item = cart[user].FirstOrDefault(ci => ci.Coupon.Id == coupon.Id);
            if (item != null)
            {
                item.Quantity--;
                if (item.Quantity <= 0)
                    cart[user].Remove(item);
            }
        }
        public static void PlaceOrder(RegisteredUser user)
        {
            if (!cart.ContainsKey(user) || cart[user].Count == 0)
                throw new InvalidOperationException("Кошик порожній.");

            var cartItems = cart[user];
            double total = cartItems.Sum(ci => ci.TotalPrice);

            if (user.Balance < total)
                throw new InvalidOperationException("Недостатньо коштів.");

            foreach (var item in cartItems)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    if (!item.Coupon.IsValid())
                    {
                        throw new ArgumentException($"Купон {item.Coupon.Name} більше не дійсний і буде пропущений.");
                    }

                    var order = new Order(user, item.Coupon, DateTime.Now);
                    if (!order.ConfirmPurchase())
                        throw new ArgumentException($"Не вдалося купити купон: {item.Coupon.Name}");
                }
            }


            user.Balance -= total;
            ClearCart(user);


            
        }
        //public static void SaveOrders(string filePath)
        //{
        //    var allOrders = RegisteredUser.GetAllUsers().SelectMany(u => u.PurchasedCoupons).ToList();
        //    var json = JsonSerializer.Serialize(allOrders, new JsonSerializerOptions
        //    {
        //        WriteIndented = true,
        //        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
        //    });
        //    File.WriteAllText(filePath, json);
        //}

        //public static void LoadOrders(string filePath)
        //{
        //    if (!File.Exists(filePath)) return;

        //    var json = File.ReadAllText(filePath);
        //    var orders = JsonSerializer.Deserialize<List<Order>>(json, new JsonSerializerOptions
        //    {
        //        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
        //    });

        //    if (orders != null)
        //    {
        //        foreach (var order in orders)
        //        {
        //            order.User?.PurchasedCoupons.Add(order);
        //        }
        //    }
        //}
    }

    public class CartItemModel
    {
        public Coupon Coupon { get; set; }
        public int Quantity { get; set; }

        public string Name => Coupon.Name;
        public string ImagePath => Coupon.ImagePath;
        public string PriceText => $"{Coupon.Price * Quantity} грн";
        public double TotalPrice => Coupon.Price * Quantity;
    }

}

