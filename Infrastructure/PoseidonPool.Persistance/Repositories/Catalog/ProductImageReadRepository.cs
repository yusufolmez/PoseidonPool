using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class ProductImageReadRepository : ReadRepository<ProductImage>, IProductImageReadRepository
    {
        public ProductImageReadRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
