using PoseidonPool.Application.DTOs.Order;

namespace PoseidonPool.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrderDTO createOrder);
        Task<List<ListOrderDTO>> GetAllOrdersAsync();
        Task<SingleOrderDTO> GetOrderByIdAsync(string id);
        Task<(bool, CompletedOrderDTO)> CompleteOrderAsync(string id);
        Task<List<ListOrderDTO>> GetMyOrdersAsync();
        Task<List<ListOrderDTO>> GetByStatusAsync(string status);
        Task<bool> CancelOrderAsync(string id);
        Task<bool> ShipOrderAsync(string id);
        Task<bool> DeliverOrderAsync(string id);
        Task<List<object>> GetOrderDetailsAsync(string id);
    }
}
