using PoseidonPool.Domain.Entities.Identity;
using PoseidonPool.Domain.Entities.Order;

namespace PoseidonPool.Domain.Entities.Cargo 
{
    public class CargoDetail : BaseEntity
    {
        public AppUser CustomerId { get; set; }
        public Guid OrderingId { get; set; }
        public string ReceiverCustomer { get; set; }
        public Guid CargoCompanyId { get; set; }
        public Ordering Ordering { get; set; }
        public CargoCompany CargoCompany { get; set; }
        public ICollection<CargoOperation> Operations { get; set; }
    }
}
