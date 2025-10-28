using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class SpecialOfferWriteRepository : WriteRepository<SpecialOffer>, ISpecialOfferWriteRepository
    {
        public SpecialOfferWriteRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
