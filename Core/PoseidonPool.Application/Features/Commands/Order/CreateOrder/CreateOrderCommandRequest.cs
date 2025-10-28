using MediatR;
using PoseidonPool.Application.DTOs.Order;

namespace PoseidonPool.Application.Features.Commands.Order.CreateOrder
{
    public class CreateOrderCommandRequest : IRequest<CreateOrderCommandResponse>
    {
        public CreateOrderDTO CreateOrder { get; set; }
    }
}
