using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PoseidonPool.Domain.Entities.Catalog
{
    public class ProductImage : BaseEntity
    {
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
