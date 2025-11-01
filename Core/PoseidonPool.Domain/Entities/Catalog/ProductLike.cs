using PoseidonPool.Domain.Entities;
using PoseidonPool.Domain.Entities.Identity;

namespace PoseidonPool.Domain.Entities.Catalog
{
    public class ProductLike : BaseEntity
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}

