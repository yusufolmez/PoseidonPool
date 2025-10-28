using PoseidonPool.Application.Repositories.Basket;

namespace PoseidonPool.Persistance.Repositories.Basket
{
    public class BasketWriteRepository : WriteRepository<PoseidonPool.Domain.Entities.Basket.Basket>, IBasketWriteRepository
    {
        public BasketWriteRepository(PoseidonPool.Persistance.Contexts.PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
