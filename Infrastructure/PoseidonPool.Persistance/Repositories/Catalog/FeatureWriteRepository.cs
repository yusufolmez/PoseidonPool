using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class FeatureWriteRepository : WriteRepository<Feature>, IFeatureWriteRepository
    {
        public FeatureWriteRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
