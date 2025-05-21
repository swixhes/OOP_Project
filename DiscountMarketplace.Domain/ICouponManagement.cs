using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountMarketplace.Domain
{
    public interface ICouponManagement
    {
        bool CreateOrEditCoupon(Coupon updatedCoupon);
    }

}
