using PoseidonPool.Application.DTOs.Order;

namespace PoseidonPool.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrderDTO createOrder);
        Task<List<ListOrderDTO>> GetAllOrdersAsync();
        Task<SingleOrderDTO> GetOrderByIdAsync(string id);
        Task<(bool, CompletedOrderDTO)> CompleteOrderAsync(string id);
    }
}
