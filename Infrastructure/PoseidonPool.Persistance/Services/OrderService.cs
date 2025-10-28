using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PoseidonPool.Application.Abstractions.Services;
using PoseidonPool.Application.DTOs.Order;
using PoseidonPool.Application.Repositories.Order;
using PoseidonPool.Domain.Entities.Order;

namespace PoseidonPool.Persistance.Services
{
    public class OrderService : IOrderService
    {
    private readonly IOrderingWriteRepository _orderingWriteRepository;
    private readonly IOrderingReadRepository _orderingReadRepository;
    private readonly IBasketService _basketService;
    private readonly UserManager<PoseidonPool.Domain.Entities.Identity.AppUser> _userManager;
    private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;

        public OrderService(
            IOrderingWriteRepository orderingWriteRepository,
            IOrderingReadRepository orderingReadRepository,
            IBasketService basketService,
            UserManager<PoseidonPool.Domain.Entities.Identity.AppUser> userManager,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            _orderingWriteRepository = orderingWriteRepository;
            _orderingReadRepository = orderingReadRepository;
            _basketService = basketService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateOrderAsync(CreateOrderDTO createOrder)
        {
            // 1. BasketService'ten mevcut kullanıcının sepetindeki ürünleri al.
            var basketItems = await _basketService.GetBasketItemsAsync();
            if (basketItems == null || !basketItems.Any())
                throw new InvalidOperationException("Basket is empty.");

            // 2. Yeni bir sipariş nesnesi oluştur.
            // resolve current user and attach to order/address so FK columns are populated
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            PoseidonPool.Domain.Entities.Identity.AppUser? currentUser = null;
            if (!string.IsNullOrEmpty(username))
            {
                currentUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            }

            var order = new Ordering
            {
                Id = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                ShippingAddress = new Domain.Entities.Order.Address
                {
                    Name = createOrder.Address.Name,
                    Surname = createOrder.Address.Surname,
                    Email = createOrder.Address.Email,
                    Phone = createOrder.Address.Phone,
                    Country = createOrder.Address.Country,
                    District = createOrder.Address.District,
                    City = createOrder.Address.City,
                    Detail1 = createOrder.Address.Detail1,
                    Detail2 = createOrder.Address.Detail2,
                    Description = createOrder.Address.Description,
                    ZipCode = createOrder.Address.ZipCode
                },
                // 3. Sepet ürünlerini sipariş detaylarına dönüştür.
                OrderDetails = basketItems.Select(bi => new OrderDetail
                {
                    ProductId = bi.ProductId,
                    ProductName = bi.Product.ProductName,
                    ProductUnitPrice = bi.Product.ProductPrice,
                    Quantity = bi.Quantity
                }).ToList(),
                TotalAmount = basketItems.Sum(bi => bi.Product.ProductPrice * bi.Quantity)
            };

            if (currentUser != null)
            {
                order.CustomerId = currentUser;
                order.ShippingAddress.CustomerId = currentUser;
            }

            // 4. Siparişi veritabanına ekle.
            await _orderingWriteRepository.AddAsync(order);
            await _orderingWriteRepository.SaveAsync();

            // 5. Sepeti temizle.
            foreach (var item in basketItems)
            {
                await _basketService.RemoveBasketItemAsync(item.Id.ToString());
            }
        }

        public async Task<List<ListOrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderingReadRepository.GetAll(false)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return orders.Select(o => new ListOrderDTO
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Completed = o.Status == OrderStatus.Delivered || o.Status == OrderStatus.Cancelled
            }).ToList();
        }

        public async Task<SingleOrderDTO> GetOrderByIdAsync(string id)
        {
            var order = await _orderingReadRepository.Table
                .Include(o => o.ShippingAddress)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

            if (order == null) return null;

            return new SingleOrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Completed = order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Cancelled,
                Address = new Application.DTOs.Order.AddressDTO
                {
                    Name = order.ShippingAddress.Name,
                    Surname = order.ShippingAddress.Surname,
                    Email = order.ShippingAddress.Email,
                    Phone = order.ShippingAddress.Phone,
                    Country = order.ShippingAddress.Country,
                    District = order.ShippingAddress.District,
                    City = order.ShippingAddress.City,
                    Detail1 = order.ShippingAddress.Detail1,
                    Detail2 = order.ShippingAddress.Detail2,
                    Description = order.ShippingAddress.Description,
                    ZipCode = order.ShippingAddress.ZipCode
                },
                BasketItems = order.OrderDetails.Select(od => new
                {
                    od.ProductId,
                    od.ProductName,
                    od.ProductUnitPrice,
                    od.Quantity
                }).ToList()
            };
        }

        public async Task<(bool, CompletedOrderDTO)> CompleteOrderAsync(string id)
        {
            Ordering order = await _orderingReadRepository.Table
                                 .Include(o => o.ShippingAddress)
                                 .FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

            if (order != null && order.Status == OrderStatus.Pending)
            {
                order.Status = OrderStatus.Paid; // Ödendi olarak işaretle
                await _orderingWriteRepository.SaveAsync();

                return (true, new CompletedOrderDTO
                {
                    OrderDate = order.OrderDate,
                    Email = order.ShippingAddress.Email
                });
            }
            return (false, null);
        }
    }
}