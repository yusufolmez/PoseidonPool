using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class ProductReadRepository : ReadRepository<Product>, IProductReadRepository
    {
        public ProductReadRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
