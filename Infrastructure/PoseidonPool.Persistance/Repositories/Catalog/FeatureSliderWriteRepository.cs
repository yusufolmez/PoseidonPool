using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class FeatureSliderWriteRepository : WriteRepository<FeatureSlider>, IFeatureSliderWriteRepository
    {
        public FeatureSliderWriteRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
