using PoseidonPool.Application.Repositories.Basket;
using PoseidonPool.Domain.Entities.Basket;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Basket
{
    public class BasketItemReadRepository : ReadRepository<BasketItem>, IBasketItemReadRepository
    {
        public BasketItemReadRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
