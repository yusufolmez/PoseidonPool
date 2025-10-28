using PoseidonPool.Application.Repositories.Order;
using PoseidonPool.Domain.Entities.Order;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Order
{
    public class AddressReadRepository : ReadRepository<Address>, IAddressReadRepository
    {
        public AddressReadRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
