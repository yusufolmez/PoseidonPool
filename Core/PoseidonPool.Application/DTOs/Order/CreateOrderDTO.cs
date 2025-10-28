using PoseidonPool.Domain.Entities.Identity;

namespace PoseidonPool.Application.DTOs.Order
{
    public class CreateOrderDTO
    {
        public string Description { get; set; }
        public AddressDTO Address { get; set; }
    }
}