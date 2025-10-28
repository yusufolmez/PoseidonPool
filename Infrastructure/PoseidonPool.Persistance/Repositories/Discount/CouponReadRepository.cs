using PoseidonPool.Application.Repositories.Discount;
using PoseidonPool.Domain.Entities.Discount;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Discount
{
    public class CouponReadRepository : ReadRepository<Coupon>, ICouponReadRepository
    {
        public CouponReadRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
