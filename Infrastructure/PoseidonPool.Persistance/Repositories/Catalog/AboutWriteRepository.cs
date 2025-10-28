using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class AboutWriteRepository : WriteRepository<About>, IAboutWriteRepository
    {
        public AboutWriteRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
