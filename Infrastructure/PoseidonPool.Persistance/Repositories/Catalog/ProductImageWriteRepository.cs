using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class ProductImageWriteRepository : WriteRepository<ProductImage>, IProductImageWriteRepository
    {
        public ProductImageWriteRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
