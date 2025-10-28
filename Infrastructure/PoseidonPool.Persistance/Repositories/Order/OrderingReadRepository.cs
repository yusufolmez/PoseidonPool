using PoseidonPool.Application.Repositories.Order;
using PoseidonPool.Domain.Entities.Order;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Order
{
    public class OrderingReadRepository : ReadRepository<Ordering>, IOrderingReadRepository
    {
        public OrderingReadRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
