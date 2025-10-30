using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Order.GetOrderDetails
{
    public class GetOrderDetailsHandler : IRequestHandler<GetOrderDetailsQueryRequest, GetOrderDetailsQueryResponse>
    {
        private readonly IOrderService _orderService;
        public GetOrderDetailsHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetOrderDetailsQueryResponse> Handle(GetOrderDetailsQueryRequest request, CancellationToken cancellationToken)
        {
            var items = await _orderService.GetOrderDetailsAsync(request.Id);
            return new GetOrderDetailsQueryResponse { Items = items };
        }
    }
}


