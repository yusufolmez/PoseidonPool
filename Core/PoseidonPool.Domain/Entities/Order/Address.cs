using PoseidonPool.Domain.Entities.Identity;

namespace PoseidonPool.Domain.Entities.Order
{
    public class Address : BaseEntity
    {
        public AppUser CustomerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Detail1 { get; set; }
        public string Detail2 { get; set; }
        public string Description { get; set; }
        public string ZipCode { get; set; }
    }
}
