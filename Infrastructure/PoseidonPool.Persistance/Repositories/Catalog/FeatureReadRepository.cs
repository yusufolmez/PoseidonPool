using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class FeatureReadRepository : ReadRepository<Feature>, IFeatureReadRepository
    {
        public FeatureReadRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
