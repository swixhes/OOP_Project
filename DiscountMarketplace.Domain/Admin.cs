using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;



namespace DiscountMarketplace.Domain
{
    public class Admin : User, IReviewManagement, ICouponManagement
    {
        private List<RegisteredUser> blockedUsers = new List<RegisteredUser>();

        public IReadOnlyList<RegisteredUser> BlockedUsers => blockedUsers.AsReadOnly();
        public event EventHandler<string> Notification;

        private string password;
        public string Password => password;

        public Admin(int id, string email, string firstName, string lastName, string phoneNumber, string password)
            : base(id, email, firstName, lastName, phoneNumber)
        {
            throw new NotImplementedException();
        }

        public override bool Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public bool DeleteReview(int reviewId)
        {
            throw new NotImplementedException();
        }

        public bool BlockUser(int userId)
        {
            throw new NotImplementedException();
        }

        public bool UnblockUser(int userId)
        {
            throw new NotImplementedException();
        }

        public bool CreateDiscount(int couponId, double discountPercentage)
        {
            throw new NotImplementedException();
        }

        public bool EditCoupon(int couponId, Coupon updatedCoupon)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCoupon(int couponId)
        {
            throw new NotImplementedException();
        }
    }
}
