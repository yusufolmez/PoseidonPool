using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PoseidonPool.Domain.Entities.Catalog
{
    public class Feature : BaseEntity
    {
        public string Title { get; set; }
        public string Icon { get; set; }
    }
}
