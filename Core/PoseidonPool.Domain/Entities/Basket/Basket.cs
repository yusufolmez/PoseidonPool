using PoseidonPool.Domain.Entities.Identity;

namespace PoseidonPool.Domain.Entities.Basket
{
    public class Basket : BaseEntity
    {
        public string CustomerId { get; set; }
        public AppUser User { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }
    }
}
