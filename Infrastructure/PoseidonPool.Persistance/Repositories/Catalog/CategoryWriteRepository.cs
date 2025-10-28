using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class CategoryWriteRepository : WriteRepository<Category>, ICategoryWriteRepository
    {
        public CategoryWriteRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
