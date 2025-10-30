using MediatR;

namespace PoseidonPool.Application.Features.Commands.Order.ShipOrder
{
    public class ShipOrderCommandRequest : IRequest<ShipOrderCommandResponse>
    {
        public string Id { get; set; }
    }
}


