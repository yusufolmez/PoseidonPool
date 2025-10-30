using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Order.GetByStatus
{
    public class GetOrdersByStatusHandler : IRequestHandler<GetOrdersByStatusQueryRequest, GetOrdersByStatusQueryResponse>
    {
        private readonly IOrderService _orderService;
        public GetOrdersByStatusHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetOrdersByStatusQueryResponse> Handle(GetOrdersByStatusQueryRequest request, CancellationToken cancellationToken)
        {
            var list = await _orderService.GetByStatusAsync(request.Status);
            return new GetOrdersByStatusQueryResponse { Orders = list };
        }
    }
}


