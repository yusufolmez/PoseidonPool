namespace PoseidonPool.Domain.Entities.Order
{
    public class OrderDetail : BaseEntity
    {
        public int OrderDetailId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public int Quantity { get; set; }
        public string OrderingId { get; set; }
        public Ordering Ordering { get; set; }
    }
}
