using System;

namespace PoseidonPool.Application.DTOs.Basket
{
    public class GuestBasketItemDTO
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }
    }
}


