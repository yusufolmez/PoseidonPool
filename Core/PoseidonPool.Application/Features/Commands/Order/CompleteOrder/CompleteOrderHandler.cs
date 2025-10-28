using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderHandler : IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
    {
        private readonly IOrderService _orderService;

        public CompleteOrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var (success, dto) = await _orderService.CompleteOrderAsync(request.Id);
            return new CompleteOrderCommandResponse
            {
                Success = success,
                Result = dto
            };
        }
    }
}
