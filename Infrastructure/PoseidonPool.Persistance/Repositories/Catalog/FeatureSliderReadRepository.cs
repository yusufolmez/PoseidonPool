using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class FeatureSliderReadRepository : ReadRepository<FeatureSlider>, IFeatureSliderReadRepository
    {
        public FeatureSliderReadRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
