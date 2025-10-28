using PoseidonPool.Application.Repositories.Order;
using PoseidonPool.Domain.Entities.Order;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Order
{
    public class OrderingWriteRepository : WriteRepository<Ordering>, IOrderingWriteRepository
    {
        public OrderingWriteRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
