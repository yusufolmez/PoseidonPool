using PoseidonPool.Application.Repositories.Basket;
using PoseidonPool.Domain.Entities.Basket;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Basket
{
    public class BasketItemWriteRepository : WriteRepository<BasketItem>, IBasketItemWriteRepository
    {
        public BasketItemWriteRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
