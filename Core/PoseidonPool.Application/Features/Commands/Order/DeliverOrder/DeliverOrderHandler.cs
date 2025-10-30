using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Order.DeliverOrder
{
    public class DeliverOrderHandler : IRequestHandler<DeliverOrderCommandRequest, DeliverOrderCommandResponse>
    {
        private readonly IOrderService _orderService;
        public DeliverOrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<DeliverOrderCommandResponse> Handle(DeliverOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var ok = await _orderService.DeliverOrderAsync(request.Id);
            return new DeliverOrderCommandResponse { Success = ok };
        }
    }
}


