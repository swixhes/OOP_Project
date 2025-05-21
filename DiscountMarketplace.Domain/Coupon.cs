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

        public int Id { get;}
        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Назва не може бути порожньою.");
                name = value;
            }
        }
        public CouponCategory Category { get; set; }
        private DateTime expirationDate;
        public DateTime ExpirationDate
        {
            get => expirationDate;
            set => expirationDate = value;
        }
        private double discount;
        public double Discount
        {
            get => discount;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("Знижка має бути від 0 до 100%.");
                discount = value;
            }
        }
        private int usageLimit;
        public int UsageLimit
        {
            get => usageLimit;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Ліміт використань не може бути від’ємним.");
                usageLimit = value;
            }
        }
        public string Description { get; set; }
        public int PurchaseCount { get; private set; } = 0;

        public double Price => 100.0 * (1 - Discount / 100.0);
        public string ImagePath { get; set; }

        public Coupon(int id, string name, CouponCategory category, DateTime expirationDate, double discount, int usageLimit, string imagePath, string description)
        {
            if (id <= 0) throw new ArgumentException("ID має бути більше 0.");

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
                PurchaseCount++;
                JsonStorage.SaveCouponsToJson(Coupon.GetAllCoupons());
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
           
            new Coupon(1, "Знижка на піцу", CouponCategory.Food, DateTime.Now.AddDays(10), 20, 5, "Images/food_coupon.jpg", "Соковита піца з сиром та ковбаскою – зі знижкою 20%!");
            new Coupon(2, "Спа-процедури", CouponCategory.Beauty, DateTime.Now.AddDays(15), 30, 1, "Images/beauty_coupon.jpg", "Розслабся у найкращому СПА-комплексі твого міста.");
            new Coupon(3, "Тур до Карпат", CouponCategory.Travel, DateTime.Now.AddDays(-1), 25, 30, "Images/travel_coupon.jpg", "Насолодись горами, свіжим повітрям та пригодами!");
            new Coupon(4, "Абонемент в спортзал", CouponCategory.Sport, DateTime.Now.AddDays(5), 40, 20, "Images/sport_coupon.jpg", "Зроби перший крок до здорового життя вже сьогодні!");
            new Coupon(5, "Кінотеатр IMAX", CouponCategory.Entertainment, DateTime.Now.AddDays(7), 15, 60, "Images/entertainment_coupon.jpg", "Найновіші фільми з ефектом повного занурення.");
            new Coupon(6, "Знижка на ковзанку", CouponCategory.Entertainment, DateTime.Now.AddDays(15), 30, 30, "Images/ice_coupon.jpg", "З купоном ви отримуєте 30% знижки на вхідний квиток на відвідування льодової ковзанки у будь-який день у ТРЦ Dream.");
            new Coupon(7, "Знижка на суші-сет", CouponCategory.Food, DateTime.Now.AddDays(7), 50, 200, "Images/sushi_coupon.jpg", "Sushi Story — мережа суші-магазинів, раніше відома як Суші Вок, яка стала улюбленою для багатьох поціновувачів японської кухні.\r\n\r\nВ закладах широкий асортимент суші, звичайних і запечених ролів, Wok-страв, піци, супів і навіть бізнес-ланчів. Для вегетаріанців передбачені спеціальні страви, а для тих, хто хоче самостійно приготувати суші вдома, у фірмовому рибному магазині завжди є свіжа риба, овочі та рис для суші.\r\n\r\nМережа Суші сторі відома якісним обслуговуванням, зручним сервісом доставки, доступними цінами та різноманітними акціями. Магазини представлені в багатьох містах України: Суші сторі Київ, Львів, Одеса, Вінниця, Суші сторі Дніпро та інші. \r\n\r\nДодатково оплачується:\r\n\r\nУпаковка (пакет, прибори та ємність у яку комплектується продукція).\r\nДоставка - умови уточнюйте у оператора чи на сайті партнера, вони залежать від філії, де хочете зробити замовлення.");
            new Coupon(8, "Манікюр зі знижкою 35%", CouponCategory.Beauty, DateTime.Now.AddDays(12), 35, 80, "Images/nails_coupon.png", "Професійний манікюр з покриттям гель-лаком у топовому салоні міста – зі знижкою 35%! Ідеальний вигляд рук за приємною ціною.");
            new Coupon(9, "Бургер-сет 1+1 у подарунок", CouponCategory.Food, DateTime.Now.AddDays(9), 50, 100, "Images/burger_coupon.jpg", "Соковиті бургери, картопля фрі та напій – купуй один набір і отримай другий у подарунок! Смакуй разом із друзями.");
            new Coupon(10, "Вікенд у Львові", CouponCategory.Travel, DateTime.Now.AddDays(30), 20, 25, "Images/lviv_coupon.jpeg", "Дводенний тур у серце Західної України з гідом, проживанням і кавовими екскурсіями. Відчуй атмосферу старовинного Львова.");
            new Coupon(11, "Йога на даху", CouponCategory.Sport, DateTime.Now.AddDays(5), 20, 100, "Images/yoga_rooftop.jpg", "Практика йоги на даху хмарочоса з видом на місто – відчуй гармонію тіла та душі. Для новачків і досвідчених.");
            new Coupon(12, "Квест-кімната «Лабораторія»", CouponCategory.Entertainment, DateTime.Now.AddDays(14), 25, 25, "Images/quest_coupon.jpg", "Захоплива пригода у науковій лабораторії. Командна гра з логічними загадками та спецефектами. Успій втекти до завершення часу!");
            new Coupon(13, "Дитячий парк «Веселка»", CouponCategory.Entertainment, DateTime.Now.AddDays(18), 30, 20, "Images/kidspark_coupon.png", "Безлімітний доступ до атракціонів, батутів і ігрових зон упродовж цілого дня! Знижка для маленьких непосид.");
            new Coupon(14, "Комплекс процедур «Glow Up»", CouponCategory.Beauty, DateTime.Now.AddDays(14), 35, 40, "Images/glowup_coupon.jpg", "Подаруй своїй шкірі сяйво: глибоке очищення, зволоження та масаж обличчя у салоні преміум-класу. Ідеально перед святами чи фотосесією.");
            new Coupon(15, "Веганський ланч-сет", CouponCategory.Food, DateTime.Now.AddDays(7), 25, 100, "Images/vegan_coupon.jpg", "Корисна і смачна страва без м’яса – боул з тофу, салат з нутом та лимонад. Ідеально для здорового способу життя!");
            new Coupon(16, "Тур у Кам’янці-Подільському", CouponCategory.Travel, DateTime.Now.AddDays(25), 30, 30, "Images/kamianets_coupon.jpg", "Старовинна фортеця, історичні місця та прогулянки містом-фестивалем – чудовий відпочинок в Україні.");
            new Coupon(17, "Кавовий курс «Бариста»", CouponCategory.Entertainment, DateTime.Now.AddDays(5), 20, 50, "Images/barista_coupon.jpg", "Дізнайся, як готувати ідеальне еспресо, латте-арт та види кави. Практика, сертифікат і море аромату!");
            new Coupon(18, "Альпака-тур у Карпатах", CouponCategory.Travel, DateTime.Now.AddDays(21), 20, 35, "Images/alpaca_coupon.jpg", "Незвичайна прогулянка з альпаками на фоні гір. Фото, обійми, емоції – чудовий варіант для пари чи родини!");
            new Coupon(19, "Заняття з плавання", CouponCategory.Sport, DateTime.Now.AddDays(6), 50, 30, "Images/swimming_coupon.jpg", "Індивідуальні чи групові заняття для дітей та дорослих. Басейн із сучасною інфраструктурою.");
            new Coupon(20, "Квитки у планетарій", CouponCategory.Entertainment, DateTime.Now.AddDays(12), 20, 120, "Images/planetarium_coupon.jpg", "Вражаюче зоряне шоу для всієї родини. Поринь у світ космосу, планет та галактик на великому куполі.");
        }

        public void RestoreUsage()
       {
           UsageLimit++;
            if (PurchaseCount > 0)
                PurchaseCount--;
            JsonStorage.SaveCouponsToJson(Coupon.GetAllCoupons());
        }
    }
}



