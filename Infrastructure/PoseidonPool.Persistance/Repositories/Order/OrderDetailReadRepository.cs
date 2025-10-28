using PoseidonPool.Application.Repositories.Order;
using PoseidonPool.Domain.Entities.Order;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Order
{
    public class OrderDetailReadRepository : ReadRepository<OrderDetail>, IOrderDetailReadRepository
    {
        public OrderDetailReadRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
