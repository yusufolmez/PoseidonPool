using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Order.CancelOrder
{
    public class CancelOrderHandler : IRequestHandler<CancelOrderCommandRequest, CancelOrderCommandResponse>
    {
        private readonly IOrderService _orderService;
        public CancelOrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<CancelOrderCommandResponse> Handle(CancelOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var ok = await _orderService.CancelOrderAsync(request.Id);
            return new CancelOrderCommandResponse { Success = ok };
        }
    }
}


