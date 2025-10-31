using System.ComponentModel.DataAnnotations;
using MediatR;

namespace PoseidonPool.Application.Features.Commands.Payment.Callback
{
    public class PaymentCallbackCommandRequest : IRequest<PaymentCallbackCommandResponse>
    {
        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; }
        
        public string ConversationId { get; set; }
    }
}


