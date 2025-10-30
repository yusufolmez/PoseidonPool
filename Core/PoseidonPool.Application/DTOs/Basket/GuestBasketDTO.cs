using System.Collections.Generic;

namespace PoseidonPool.Application.DTOs.Basket
{
    public class GuestBasketDTO
    {
        public string GuestId { get; set; }
        public List<GuestBasketItemDTO> Items { get; set; } = new List<GuestBasketItemDTO>();
    }
}


