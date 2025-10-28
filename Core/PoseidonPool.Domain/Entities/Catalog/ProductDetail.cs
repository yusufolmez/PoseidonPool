using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PoseidonPool.Domain.Entities.Catalog
{
    public class ProductDetail : BaseEntity
    {
        public string ProductDescription { get; set; }
        public string ProductInfo { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
