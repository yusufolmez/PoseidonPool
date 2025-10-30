using System.Collections.Generic;
using PoseidonPool.Application.DTOs.Basket;

namespace PoseidonPool.Application.Features.Queries.Basket.Guest.GetGuestItems
{
    public class GetGuestItemsQueryResponse
    {
        public List<GuestBasketItemDTO> Items { get; set; }
    }
}


