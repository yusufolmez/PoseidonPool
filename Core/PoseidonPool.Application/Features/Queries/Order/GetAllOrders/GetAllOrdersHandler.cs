using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Order.GetAllOrders
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQueryRequest, GetAllOrdersQueryResponse>
    {
        private readonly IOrderService _orderService;

        public GetAllOrdersHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetAllOrdersQueryResponse> Handle(GetAllOrdersQueryRequest request, CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return new GetAllOrdersQueryResponse
            {
                Orders = orders
            };
        }
    }
}
