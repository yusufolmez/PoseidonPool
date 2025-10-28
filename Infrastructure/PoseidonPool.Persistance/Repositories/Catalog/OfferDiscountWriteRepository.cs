using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class OfferDiscountWriteRepository : WriteRepository<OfferDiscount>, IOfferDiscountWriteRepository
    {
        public OfferDiscountWriteRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
