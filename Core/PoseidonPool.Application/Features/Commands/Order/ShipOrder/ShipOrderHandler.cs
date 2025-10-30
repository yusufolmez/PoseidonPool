using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Order.ShipOrder
{
    public class ShipOrderHandler : IRequestHandler<ShipOrderCommandRequest, ShipOrderCommandResponse>
    {
        private readonly IOrderService _orderService;
        public ShipOrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<ShipOrderCommandResponse> Handle(ShipOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var ok = await _orderService.ShipOrderAsync(request.Id);
            return new ShipOrderCommandResponse { Success = ok };
        }
    }
}


