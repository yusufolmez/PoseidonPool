using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class ContactWriteRepository : WriteRepository<Contact>, IContactWriteRepository
    {
        public ContactWriteRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
