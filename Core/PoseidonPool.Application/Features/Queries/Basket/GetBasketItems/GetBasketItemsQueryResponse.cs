using System.Collections.Generic;
using PoseidonPool.Domain.Entities.Basket;

namespace PoseidonPool.Application.Features.Queries.Basket.GetBasketItems
{
    public class GetBasketItemsQueryResponse
    {
        public List<BasketItem> Items { get; set; }
    }
}
