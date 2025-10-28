using PoseidonPool.Application.DTOs.Order;
using System.Collections.Generic;

namespace PoseidonPool.Application.Features.Queries.Order.GetAllOrders
{
    public class GetAllOrdersQueryResponse
    {
        public List<ListOrderDTO> Orders { get; set; }
    }
}
