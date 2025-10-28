using PoseidonPool.Domain.Entities.Catalog;

namespace PoseidonPool.Domain.Entities.Order
{
    public class OrderDetail : BaseEntity
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public int Quantity { get; set; }
        public Guid OrderingId { get; set; }
        public Product Product { get; set; }
        public Ordering Ordering { get; set; }
    }
}
