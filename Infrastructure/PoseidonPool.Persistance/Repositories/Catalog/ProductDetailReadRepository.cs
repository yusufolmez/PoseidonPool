using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class ProductDetailReadRepository : ReadRepository<ProductDetail>, IProductDetailReadRepository
    {
        public ProductDetailReadRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
