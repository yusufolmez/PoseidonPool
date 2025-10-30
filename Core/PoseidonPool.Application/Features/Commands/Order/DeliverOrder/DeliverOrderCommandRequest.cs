using MediatR;

namespace PoseidonPool.Application.Features.Commands.Order.DeliverOrder
{
    public class DeliverOrderCommandRequest : IRequest<DeliverOrderCommandResponse>
    {
        public string Id { get; set; }
    }
}


