using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Order.GetMyOrders
{
    public class GetMyOrdersHandler : IRequestHandler<GetMyOrdersQueryRequest, GetMyOrdersQueryResponse>
    {
        private readonly IOrderService _orderService;
        public GetMyOrdersHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetMyOrdersQueryResponse> Handle(GetMyOrdersQueryRequest request, CancellationToken cancellationToken)
        {
            var list = await _orderService.GetMyOrdersAsync();
            return new GetMyOrdersQueryResponse { Orders = list };
        }
    }
}


