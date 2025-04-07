using DiscountMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiscountMarketplace.Domain
{
    public delegate void PurchaseHandler(string message);

    public class RegisteredUser : User, IComparable<RegisteredUser>, IReviewManagement
    {
        public List<Order> PurchasedCoupons { get; private set; }
        public double Balance { get; set; }
        public int Id { get; internal set; }

        public static RegisteredUser GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<CouponEventArgs> CouponPurchased;
        public event EventHandler<CouponEventArgs> CouponReturned;
        public event EventHandler<string> Notification;
        public event PurchaseHandler OnSuccessfulPurchase;

        public RegisteredUser(int id, string email, string firstName, string lastName, string phoneNumber, string password, double initialBalance)
            : base(id, email, firstName, lastName, phoneNumber)
        {
            throw new NotImplementedException();
        }

        public override bool Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public bool PurchaseCoupon(Coupon coupon)
        {
            throw new NotImplementedException();
        }

        public List<Order> ViewPurchasedCoupons()
        {
            throw new NotImplementedException();
        }

        public bool ReturnCoupon(int couponId)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(RegisteredUser other)
        {
            throw new NotImplementedException();
        }

        public bool DeleteReview(int reviewId)
        {
            throw new NotImplementedException();
        }

        public class CouponEventArgs : EventArgs
        {
            public Coupon Coupon { get; }

            public CouponEventArgs(Coupon coupon)
            {
                throw new NotImplementedException();
            }
        }
    }
}



