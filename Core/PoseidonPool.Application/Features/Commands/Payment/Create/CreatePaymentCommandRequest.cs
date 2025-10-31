using System.ComponentModel.DataAnnotations;
using MediatR;

namespace PoseidonPool.Application.Features.Commands.Payment.Create
{
    public class CreatePaymentCommandRequest : IRequest<CreatePaymentCommandResponse>
    {
        [Required(ErrorMessage = "OrderId is required")]
        public string OrderId { get; set; }
        
        public string CallbackUrl { get; set; }
    }
}


