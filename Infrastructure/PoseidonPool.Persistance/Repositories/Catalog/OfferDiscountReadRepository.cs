using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class OfferDiscountReadRepository : ReadRepository<OfferDiscount>, IOfferDiscountReadRepository
    {
        public OfferDiscountReadRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
