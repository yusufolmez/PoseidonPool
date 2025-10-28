using PoseidonPool.Domain.Entities.Basket;
using PoseidonPool.Domain.Entities.Identity;

namespace PoseidonPool.Domain.Entities.Order
{
    public enum OrderStatus
    {
        Pending,    // Sepetten yeni düştü, ödeme bekleniyor
        Paid,       // Ödendi
        Shipped,    // Kargolandı
        Delivered,  // Teslim Edildi
        Cancelled   // İptal Edildi
    }
    public class Ordering : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public Address ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public AppUser CustomerId { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
