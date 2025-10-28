using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PoseidonPool.Domain.Entities.Catalog
{
    public class Brand : BaseEntity
    {
        public string BrandName { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
