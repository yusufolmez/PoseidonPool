using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Domain.Entities.Identity;

namespace PoseidonPool.Domain.Entities.Comment
{
    public class UserComment : BaseEntity
    {
        public string CustomerId { get; set; }
        public AppUser Customer { get; set; }
        public string? ImageUrl { get; set; }
        public string CommentDetail { get; set; }
        public int Rating { get; set; }
        public bool Status { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
