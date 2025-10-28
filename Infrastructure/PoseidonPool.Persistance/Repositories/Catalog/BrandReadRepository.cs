using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class BrandReadRepository : ReadRepository<Brand>, IBrandReadRepository
    {
        public BrandReadRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
