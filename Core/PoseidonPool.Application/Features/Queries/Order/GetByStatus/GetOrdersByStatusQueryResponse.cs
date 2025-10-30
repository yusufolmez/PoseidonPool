using System.Collections.Generic;
using PoseidonPool.Application.DTOs.Order;

namespace PoseidonPool.Application.Features.Queries.Order.GetByStatus
{
    public class GetOrdersByStatusQueryResponse
    {
        public List<ListOrderDTO> Orders { get; set; }
    }
}


