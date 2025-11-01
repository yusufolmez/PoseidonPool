using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class ProductLikeWriteRepository : WriteRepository<ProductLike>, IProductLikeWriteRepository
    {
        public ProductLikeWriteRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}

