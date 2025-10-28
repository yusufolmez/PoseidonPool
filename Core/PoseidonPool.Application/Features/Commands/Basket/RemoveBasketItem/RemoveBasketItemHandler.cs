using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Basket.RemoveBasketItem
{
    public class RemoveBasketItemHandler : IRequestHandler<RemoveBasketItemCommandRequest, RemoveBasketItemCommandResponse>
    {
        private readonly IBasketService _basketService;

        public RemoveBasketItemHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<RemoveBasketItemCommandResponse> Handle(RemoveBasketItemCommandRequest request, CancellationToken cancellationToken)
        {
            await _basketService.RemoveBasketItemAsync(request.BasketItemId);
            return new RemoveBasketItemCommandResponse
            {
                Success = true,
                Message = "Basket item removed."
            };
        }
    }
}
