using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class AboutReadRepository : ReadRepository<About>, IAboutReadRepository
    {
        public AboutReadRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
