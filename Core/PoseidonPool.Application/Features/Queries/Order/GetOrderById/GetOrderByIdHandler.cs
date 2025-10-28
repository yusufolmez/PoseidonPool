using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Order.GetOrderById
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
    {
        private readonly IOrderService _orderService;

        public GetOrderByIdHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderByIdAsync(request.Id);
            return new GetOrderByIdQueryResponse
            {
                Order = order
            };
        }
    }
}
