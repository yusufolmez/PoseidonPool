using Microsoft.EntityFrameworkCore;
using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Persistance.Contexts;
using PoseidonPool.Persistance.Repositories;

namespace PoseidonPool.Persistance.Repositories.Catalog
{
    public class ProductLikeReadRepository : ReadRepository<ProductLike>, IProductLikeReadRepository
    {
        public ProductLikeReadRepository(PoseidonPoolDBContext context) : base(context)
        {
        }

        public async Task<ProductLike?> GetByUserAndProductAsync(string userId, Guid productId)
        {
            return await Table
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);
        }

        public async Task<int> GetLikeCountByProductIdAsync(Guid productId)
        {
            return await Table
                .CountAsync(x => x.ProductId == productId);
        }

        public async Task<List<ProductLike>> GetLikesByUserIdAsync(string userId)
        {
            return await Table
                .Where(x => x.UserId == userId)
                .Include(x => x.Product)
                .ThenInclude(x => x.Brand)
                .Include(x => x.Product)
                .ThenInclude(x => x.Category)
                .ToListAsync();
        }

        public async Task<bool> IsLikedByUserAsync(string userId, Guid productId)
        {
            return await Table
                .AnyAsync(x => x.UserId == userId && x.ProductId == productId);
        }
    }
}

