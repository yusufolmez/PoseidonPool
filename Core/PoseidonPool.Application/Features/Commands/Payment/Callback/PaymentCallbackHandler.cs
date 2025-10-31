using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Payment;
using PoseidonPool.Application.Abstractions.Services;
using PoseidonPool.Application.DTOs.Payment;

namespace PoseidonPool.Application.Features.Commands.Payment.Callback
{
    public class PaymentCallbackHandler : IRequestHandler<PaymentCallbackCommandRequest, PaymentCallbackCommandResponse>
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;

        public PaymentCallbackHandler(IPaymentService paymentService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
        }

        public async Task<PaymentCallbackCommandResponse> Handle(PaymentCallbackCommandRequest request, CancellationToken cancellationToken)
        {
            var callbackRequest = new CallbackRequestDTO
            {
                Token = request.Token,
                ConversationId = request.ConversationId
            };
            
            var res = await _paymentService.HandleCallbackAsync(callbackRequest);
            
            if (res.Status == "success" && !string.IsNullOrWhiteSpace(res.PaymentId))
            {
                var conversationId = res.ConversationId ?? request.ConversationId ?? callbackRequest.ConversationId;
                
                if (!string.IsNullOrWhiteSpace(conversationId))
                {
                    try
                    {
                        var orderId = conversationId;
                        if (orderId.StartsWith("PP-"))
                        {
                            orderId = orderId.Substring(3);
                        }
                        
                        if (Guid.TryParse(orderId, out var orderGuid))
                        {
                            await _orderService.CompleteOrderAsync(orderGuid.ToString());
                            
                            if (string.IsNullOrWhiteSpace(res.ConversationId))
                            {
                                res.ConversationId = conversationId;
                            }
                        }
                    }
                    catch { }
                }
            }
            
            return new PaymentCallbackCommandResponse { Result = res };
        }
    }
}


