using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PoseidonPool.Domain.Entities.Catalog
{
    public class SpecialOffer : BaseEntity
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string ImageUrl { get; set; }
    }
}
