using PoseidonPool.Application.Repositories;
using PoseidonPool.Domain.Entities.Catalog;

namespace PoseidonPool.Application.Repositories.Catalog
{
    public interface IProductLikeReadRepository : IReadRepository<ProductLike>
    {
        Task<ProductLike?> GetByUserAndProductAsync(string userId, Guid productId);
        Task<int> GetLikeCountByProductIdAsync(Guid productId);
        Task<List<ProductLike>> GetLikesByUserIdAsync(string userId);
        Task<bool> IsLikedByUserAsync(string userId, Guid productId);
    }
}

