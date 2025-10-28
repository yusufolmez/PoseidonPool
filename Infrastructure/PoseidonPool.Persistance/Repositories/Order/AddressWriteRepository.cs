using PoseidonPool.Application.Repositories.Order;
using PoseidonPool.Domain.Entities.Order;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Order
{
    public class AddressWriteRepository : WriteRepository<Address>, IAddressWriteRepository
    {
        public AddressWriteRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
