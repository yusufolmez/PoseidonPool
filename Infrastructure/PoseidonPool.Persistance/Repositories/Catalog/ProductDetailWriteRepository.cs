using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class ProductDetailWriteRepository : WriteRepository<ProductDetail>, IProductDetailWriteRepository
    {
        public ProductDetailWriteRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
