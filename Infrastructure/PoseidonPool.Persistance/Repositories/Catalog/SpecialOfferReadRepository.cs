using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class SpecialOfferReadRepository : ReadRepository<SpecialOffer>, ISpecialOfferReadRepository
    {
        public SpecialOfferReadRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
