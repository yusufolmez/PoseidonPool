using PoseidonPool.Application.DTOs.Order;

namespace PoseidonPool.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderCommandResponse
    {
        public bool Success { get; set; }
        public CompletedOrderDTO Result { get; set; }
    }
}
