using PoseidonPool.Domain.Entities.Identity;
using PoseidonPool.Domain.Entities.Order;

namespace PoseidonPool.Domain.Entities.Basket
{
    public class Basket : BaseEntity
    {
        public string BasketId { get; set; }
        public string CustomerId { get; set; }
        public AppUser User { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }
    }
}
