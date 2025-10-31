using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Payment;
using PoseidonPool.Application.Abstractions.Services;
using PoseidonPool.Application.DTOs.Payment;
using Microsoft.AspNetCore.Http;

namespace PoseidonPool.Application.Features.Commands.Payment.Create
{
    public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommandRequest, CreatePaymentCommandResponse>
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreatePaymentHandler(
            IPaymentService paymentService,
            IOrderService orderService,
            IHttpContextAccessor httpContextAccessor)
        {
            _paymentService = paymentService;
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreatePaymentCommandResponse> Handle(CreatePaymentCommandRequest request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderByIdAsync(request.OrderId);
            
            if (order == null)
            {
                throw new ArgumentException($"Order with id {request.OrderId} not found.");
            }

            if (order.Status != 0)
            {
                throw new InvalidOperationException($"Payment cannot be created for order with status {order.Status}. Only pending orders can be paid.");
            }

            var clientIp = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "127.0.0.1";

            var paymentRequest = new CreatePaymentRequestDTO
            {
                Price = order.TotalAmount,
                PaidPrice = order.TotalAmount,
                Currency = "TRY",
                ConversationId = $"PP-{order.Id}",
                CallbackUrl = request.CallbackUrl,
                Buyer = new BuyerDTO
                {
                    Id = order.Id.ToString(),
                    Name = order.Address?.Name ?? "Test",
                    Surname = order.Address?.Surname ?? "User",
                    Email = order.Address?.Email ?? "test@example.com",
                    GsmNumber = order.Address?.Phone ?? "5550000000",
                    IdentityNumber = "11111111111",
                    Ip = clientIp,
                    City = order.Address?.City ?? "Istanbul",
                    Country = order.Address?.Country ?? "Turkey",
                    Address = $"{order.Address?.Detail1 ?? ""} {order.Address?.Detail2 ?? ""}".Trim(),
                    ZipCode = order.Address?.ZipCode ?? "34000"
                },
                Items = order.BasketItems != null && order.BasketItems.Any()
                    ? order.BasketItems.Select((item, index) => new BasketItemDTO
                        {
                            Id = $"item-{index + 1}",
                            Name = item.ProductName ?? "Product",
                            Category1 = "General",
                            Category2 = "General",
                            Price = item.ProductUnitPrice * item.Quantity
                        }).ToList()
                    : new List<BasketItemDTO>()
            };

            var res = await _paymentService.CreateAsync(paymentRequest);
            return new CreatePaymentCommandResponse { Result = res };
        }
    }
}


