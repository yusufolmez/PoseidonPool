using Microsoft.AspNetCore.Identity;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Domain.Entities.Comment;

namespace PoseidonPool.Domain.Entities.Identity
{
    public class AppUser : IdentityUser<string>
    {
        public string NameSurname { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }
        public ICollection<PoseidonPool.Domain.Entities.Basket.Basket> Baskets { get; set; }
        public ICollection<UserComment> Comments { get; set; }
        public ICollection<ProductLike> ProductLikes { get; set; }
    }
}
