using PoseidonPool.Application.Repositories.Order;
using PoseidonPool.Domain.Entities.Order;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Order
{
    public class OrderDetailWriteRepository : WriteRepository<OrderDetail>, IOrderDetailWriteRepository
    {
        public OrderDetailWriteRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
