using PoseidonPool.Application.Repositories.Basket;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Basket
{
    public class BasketReadRepository : ReadRepository<PoseidonPool.Domain.Entities.Basket.Basket>, IBasketReadRepository
    {
        public BasketReadRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
