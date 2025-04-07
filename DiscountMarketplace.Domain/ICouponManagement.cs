using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountMarketplace.Domain
{
    public interface ICouponManagement
    {
        bool CreateDiscount(int couponId, double discountPercentage);
        bool EditCoupon(int couponId, Coupon updatedCoupon);
        bool DeleteCoupon(int couponId);
    }

}
