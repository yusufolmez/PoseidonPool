using PoseidonPool.Application.Repositories.Discount;
using PoseidonPool.Domain.Entities.Discount;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Discount
{
    public class CouponWriteRepository : WriteRepository<Coupon>, ICouponWriteRepository
    {
        public CouponWriteRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
