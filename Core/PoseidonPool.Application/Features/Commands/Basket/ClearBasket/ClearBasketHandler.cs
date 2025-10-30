using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Basket.ClearBasket
{
    public class ClearBasketHandler : IRequestHandler<ClearBasketCommandRequest, ClearBasketCommandResponse>
    {
        private readonly IBasketService _basketService;

        public ClearBasketHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<ClearBasketCommandResponse> Handle(ClearBasketCommandRequest request, CancellationToken cancellationToken)
        {
            await _basketService.ClearAsync();
            return new ClearBasketCommandResponse { Success = true };
        }
    }
}


