using System.Collections.Generic;
using PoseidonPool.Application.DTOs.Order;

namespace PoseidonPool.Application.Features.Queries.Order.GetMyOrders
{
    public class GetMyOrdersQueryResponse
    {
        public List<ListOrderDTO> Orders { get; set; }
    }
}


