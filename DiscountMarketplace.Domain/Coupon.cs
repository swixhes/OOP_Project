using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountMarketplace.Domain
{
    public class Coupon
    {
        private static List<Coupon> allCoupons = new List<Coupon>();

        public static void AddCoupon(Coupon coupon)
        {
            if (coupon != null && !allCoupons.Any(c => c.Id == coupon.Id))
                allCoupons.Add(coupon);
        }

        public static List<Coupon> GetAllCoupons() => allCoupons;

        public int Id { get; }
        public string Name { get; }
        public CouponCategory Category { get; }
        public DateTime ExpirationDate { get; }
        public double Discount { get; }
        public int UsageLimit { get; private set; }
        public string Description { get; }
        public int PurchaseCount { get; private set; } = 0; // кількість покупок

        public double Price => 100.0 * (1 - Discount / 100.0);
        public string ImagePath { get; }

        public Coupon(int id, string name, CouponCategory category, DateTime expirationDate, double discount, int usageLimit, string imagePath, string description, bool allowPastExpiration = false)
        {
            if (id <= 0) throw new ArgumentException("ID має бути більше 0.");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Назва не може бути порожньою.");
            if (!allowPastExpiration && expirationDate <= DateTime.Now) throw new ArgumentException("Дата закінчення дії не може бути в минулому.");
            if (discount < 0 || discount > 100) throw new ArgumentException("Знижка має бути від 0 до 100%.");
            if (usageLimit <= 0) throw new ArgumentException("Ліміт використань має бути більше 0.");

            Id = id;
            Name = name;
            Category = category;
            ExpirationDate = expirationDate;
            Discount = discount;
            UsageLimit = usageLimit;
            Description = description ?? "Опис відсутній.";
            ImagePath = imagePath;
            AddCoupon(this);
        }

        public bool IsValid() => ExpirationDate > DateTime.Now && UsageLimit > 0;

        public bool MarkAsUsed()
        {
            if (IsValid())
            {
                UsageLimit--;
                PurchaseCount++; // збільшуємо лічильник покупок
                return true;
            }
            return false;
        }

        public static List<Coupon> GetByCategory(CouponCategory category)
        {
            return allCoupons.Where(c => c.Category == category).ToList();
        }

        public static void InitializeTestCoupons()
        {
            if (allCoupons.Count > 0) return;

            new Coupon(1, "Знижка на піцу", CouponCategory.Food, DateTime.Now.AddDays(10), 20, 100, "Images/food_coupon.jpg", "Соковита піца з сиром та ковбаскою – зі знижкою 20%!");
            new Coupon(2, "Спа-процедури", CouponCategory.Beauty, DateTime.Now.AddDays(15), 30, 50, "Images/beauty_coupon.jpg", "Розслабся у найкращому СПА-комплексі твого міста.");
            new Coupon(3, "Тур до Карпат", CouponCategory.Travel, DateTime.Now.AddDays(20), 25, 30, "Images/travel_coupon.jpg", "Насолодись горами, свіжим повітрям та пригодами!");
            new Coupon(4, "Абонемент в спортзал", CouponCategory.Sport, DateTime.Now.AddDays(5), 40, 20, "Images/sport_coupon.jpg", "Зроби перший крок до здорового життя вже сьогодні!");
            new Coupon(5, "Кінотеатр IMAX", CouponCategory.Entertainment, DateTime.Now.AddDays(7), 15, 60, "Images/entertainment_coupon.jpg", "Найновіші фільми з ефектом повного занурення.");
        }
    }
}



