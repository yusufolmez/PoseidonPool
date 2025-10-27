using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoseidonPool.Application.Repositories.Order
{
    public interface IOrderDetailReadRepository : IReadRepository<Domain.Entities.Order.OrderDetail>
    {
    }
}
