using MediatR;

namespace PoseidonPool.Application.Features.Commands.Order.CancelOrder
{
    public class CancelOrderCommandRequest : IRequest<CancelOrderCommandResponse>
    {
        public string Id { get; set; }
    }
}


