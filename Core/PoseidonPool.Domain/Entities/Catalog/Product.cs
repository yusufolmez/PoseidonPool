using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PoseidonPool.Domain.Entities.Comment;

namespace PoseidonPool.Domain.Entities.Catalog
{
    public class Product : BaseEntity
    {
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public Guid ProductImageUrl { get; set; }
        public Guid ProductDetailId { get; set; }
        public Guid ProductImageId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid BrandId { get; set; }
        public ICollection<UserComment> Comments { get; set; }
        public ICollection<ProductImage> Images { get; set; }
        public ICollection<ProductLike> ProductLikes { get; set; }
        public Brand Brand { get; set; }
        public ProductDetail ProductDetail { get; set; }
        public Category Category { get; set; }
    }
}
